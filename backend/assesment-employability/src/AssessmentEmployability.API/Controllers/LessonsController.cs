using AssessmentEmployability.Application.Common;
using AssessmentEmployability.Application.DTOs.Lesson;
using AssessmentEmployability.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentEmployability.API.Controllers;

[ApiController]
[Route("api/lessons")]
[Authorize]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLessonsByCourse([FromQuery] Guid courseId)
    {
        var result = await _lessonService.GetLessonsByCourseIdAsync(courseId);

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

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateLesson([FromBody] CreateLessonRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<LessonResponseDto>.FailureResponse("Validation failed", ModelState));
        }

        var result = await _lessonService.CreateLessonAsync(request);

        if (!result.Success)
        {
            if (result.Message == "Course not found.")
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetLessonsByCourse), new { courseId = result.Data?.CourseId }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateLesson(Guid id, [FromBody] UpdateLessonRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<LessonResponseDto>.FailureResponse("Validation failed", ModelState));
        }

        var result = await _lessonService.UpdateLessonAsync(id, request);

        if (!result.Success)
        {
            if (result.Message == "Lesson not found.")
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteLesson(Guid id)
    {
        var result = await _lessonService.DeleteLessonAsync(id);

        if (!result.Success)
        {
            if (result.Message == "Lesson not found.")
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("reorder")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ReorderLessons([FromBody] List<ReorderLessonItemDto> items)
    {
        if (items == null || !items.Any())
        {
            return BadRequest(ApiResponse<bool>.FailureResponse("No items provided."));
        }

        var result = await _lessonService.ReorderLessonsAsync(items);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
