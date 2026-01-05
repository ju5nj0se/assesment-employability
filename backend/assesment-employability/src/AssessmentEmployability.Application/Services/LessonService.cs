using AssessmentEmployability.Application.Common;
using AssessmentEmployability.Application.DTOs.Lesson;
using AssessmentEmployability.Application.Interfaces;
using AssessmentEmployability.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentEmployability.Application.Services;

public class LessonService : ILessonService
{
    private readonly IUnitOfWork _unitOfWork;

    public LessonService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<IEnumerable<LessonResponseDto>>> GetLessonsByCourseIdAsync(Guid courseId)
    {
        // 1. Verify if course exists and is not deleted
        var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
        if (course == null)
        {
            return ApiResponse<IEnumerable<LessonResponseDto>>.FailureResponse("Course not found.");
        }

        // 2. Get lessons
        var lessons = await _unitOfWork.Lessons.GetByCourseIdAsync(courseId);

        // 3. Map to DTO
        var lessonDtos = lessons.Select(l => new LessonResponseDto
        {
            Id = l.Id,
            CourseId = l.CourseId,
            Title = l.Title,
            Order = l.Order,
            CreatedAt = l.CreatedAt,
            UpdatedAt = l.UpdatedAt
        });

        return ApiResponse<IEnumerable<LessonResponseDto>>.SuccessResponse(lessonDtos, "Lessons retrieved successfully");
    }

    public async Task<ApiResponse<LessonResponseDto>> CreateLessonAsync(CreateLessonRequestDto request)
    {
        // 1. Verify if course exists
        var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
        if (course == null)
        {
            return ApiResponse<LessonResponseDto>.FailureResponse("Course not found.");
        }

        // 2. Get max order and increment
        var maxOrder = await _unitOfWork.Lessons.GetMaxOrderAsync(request.CourseId);
        
        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = request.CourseId,
            Title = request.Title,
            Order = maxOrder + 1,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // 3. Save
        await _unitOfWork.Lessons.AddAsync(lesson);
        await _unitOfWork.SaveChangesAsync();

        // 4. Map to DTO
        var responseDto = new LessonResponseDto
        {
            Id = lesson.Id,
            CourseId = lesson.CourseId,
            Title = lesson.Title,
            Order = lesson.Order,
            CreatedAt = lesson.CreatedAt,
            UpdatedAt = lesson.UpdatedAt
        };

        return ApiResponse<LessonResponseDto>.SuccessResponse(responseDto, "Lesson created successfully");
    }

    public async Task<ApiResponse<LessonResponseDto>> UpdateLessonAsync(Guid id, UpdateLessonRequestDto request)
    {
        // 1. Verify if lesson exists
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);
        if (lesson == null)
        {
            return ApiResponse<LessonResponseDto>.FailureResponse("Lesson not found.");
        }

        // 2. If changing course, verify if new course exists
        if (lesson.CourseId != request.CourseId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null)
            {
                return ApiResponse<LessonResponseDto>.FailureResponse("The specified new Course was not found.");
            }
            lesson.CourseId = request.CourseId;
        }

        // 3. Update fields
        lesson.Title = request.Title;
        lesson.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Lessons.UpdateAsync(lesson);
        await _unitOfWork.SaveChangesAsync();

        // 4. Map to DTO
        var responseDto = new LessonResponseDto
        {
            Id = lesson.Id,
            CourseId = lesson.CourseId,
            Title = lesson.Title,
            Order = lesson.Order,
            CreatedAt = lesson.CreatedAt,
            UpdatedAt = lesson.UpdatedAt
        };

        return ApiResponse<LessonResponseDto>.SuccessResponse(responseDto, "Lesson updated successfully");
    }

    public async Task<ApiResponse<bool>> DeleteLessonAsync(Guid id)
    {
        // 1. Verify if lesson exists
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);
        if (lesson == null)
        {
            return ApiResponse<bool>.FailureResponse("Lesson not found.");
        }

        // 2. Soft delete
        lesson.IsDeleted = true;
        lesson.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Lessons.UpdateAsync(lesson);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Lesson deleted successfully.");
    }

    public async Task<ApiResponse<bool>> ReorderLessonsAsync(List<ReorderLessonItemDto> items)
    {
        if (items == null || !items.Any())
        {
            return ApiResponse<bool>.FailureResponse("No items provided for reordering.");
        }

        var lessonsToUpdate = new List<Lesson>();

        foreach (var item in items)
        {
            var lesson = await _unitOfWork.Lessons.GetByIdAsync(item.Id);
            if (lesson == null)
            {
                return ApiResponse<bool>.FailureResponse($"Lesson with ID {item.Id} not found.");
            }

            lesson.Order = item.Order;
            lesson.UpdatedAt = DateTime.UtcNow;
            lessonsToUpdate.Add(lesson);
        }

        await _unitOfWork.Lessons.UpdateRangeAsync(lessonsToUpdate);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Lessons reordered successfully.");
    }
}
