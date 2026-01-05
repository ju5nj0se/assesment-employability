using AssessmentEmployability.Application.Constants;
using AssessmentEmployability.Application.DTOs.Auth;
using AssessmentEmployability.Application.Interfaces;
using AssessmentEmployability.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentEmployability.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation("Attempting login for {Email}", request.Email);
        
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogWarning("Login failed: User {Email} not found", request.Email);
            throw new UnauthorizedAccessException(Messages.InvalidCredentials); // Security: don't reveal user not found
        }

        var isValid = await _userRepository.CheckPasswordAsync(user, request.Password);
        if (!isValid)
        {
            _logger.LogWarning("Login failed: Invalid password for {Email}", request.Email);
            throw new UnauthorizedAccessException(Messages.InvalidCredentials);
        }

        var roles = await _userRepository.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user, roles);

        return new AuthResponse
        {
            Token = token,
            Email = user.Email!,
            FullName = user.FullName,
            Roles = roles.ToList()
        };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        _logger.LogInformation("Attempting registration for {Email}", request.Email);

        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("Registration failed: User {Email} already exists", request.Email);
            throw new InvalidOperationException(Messages.UserAlreadyExists);
        }

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            Document = request.Document
        };

        var creationResult = await _userRepository.CreateUserAsync(user, request.Password);
        if (!creationResult.Succeeded)
        {
            var errors = string.Join(", ", creationResult.Errors);
            _logger.LogError("Registration failed for {Email}: {Errors}", request.Email, errors);
            throw new Exception($"{Messages.RegistrationFailed} {errors}");
        }

        await _userRepository.AddToRoleAsync(user, Messages.UserRoleName);
        var roles = new List<string> { Messages.UserRoleName };

        var token = _tokenService.GenerateToken(user, roles);

        return new AuthResponse
        {
            Token = token,
            Email = user.Email!,
            FullName = user.FullName,
            Roles = roles
        };
    }
}
