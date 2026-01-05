using AssessmentEmployability.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<(bool Succeeded, IEnumerable<string> Errors)> CreateUserAsync(User user, string password);
    Task AddToRoleAsync(User user, string role);
    Task<IList<string>> GetRolesAsync(User user);
}

public interface ITokenService
{
    string GenerateToken(User user, IList<string> roles);
}
