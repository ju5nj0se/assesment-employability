using AssessmentEmployability.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssessmentEmployability.Application.Interfaces;
using AssessmentEmployability.Application.DTOs.Course;
using AssessmentEmployability.Application.Common;
using System.Threading.Tasks;
using System;

namespace AssessmentEmployability.API.Controllers;

[ApiController]
[Route("api/courses")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses([FromQuery] string? status)
    {
        var result = await _courseService.GetCoursesAsync(status);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<CourseResponseDto>.FailureResponse("Validation failed", ModelState));
        }

        var result = await _courseService.CreateCourseAsync(request);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetCourses), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<CourseResponseDto>.FailureResponse("Validation failed", ModelState));
        }

        var result = await _courseService.UpdateCourseAsync(id, request);

        if (!result.Success)
        {
            if (result.Message == "Course not found.")
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPatch("{id}/publish")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PublishCourse(Guid id)
    {
        var result = await _courseService.PublishCourseAsync(id);

        if (!result.Success)
        {
            if (result.Message == "Course not found.")
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPatch("{id}/unpublish")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UnpublishCourse(Guid id)
    {
        var result = await _courseService.UnpublishCourseAsync(id);

        if (!result.Success)
        {
            if (result.Message == "Course not found.")
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCourse(Guid id)
    {
        var result = await _courseService.DeleteCourseAsync(id);

        if (!result.Success)
        {
            if (result.Message == "Course not found.")
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        return Ok(result);
    }
}