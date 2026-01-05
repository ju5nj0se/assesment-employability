using AssessmentEmployability.Application.Common;
using AssessmentEmployability.Application.DTOs.Course;
using AssessmentEmployability.Application.Interfaces;
using AssessmentEmployability.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentEmployability.Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<IEnumerable<CourseResponseDto>>> GetCoursesAsync(string? status)
    {
        IEnumerable<Course> courses;

        if (string.IsNullOrWhiteSpace(status))
        {
            courses = await _unitOfWork.Courses.GetAllAsync();
        }
        else
        {
            var allowedStatuses = new[] { "draft", "published" };
            if (!allowedStatuses.Contains(status.ToLower()))
            {
                return ApiResponse<IEnumerable<CourseResponseDto>>.FailureResponse("Invalid status filter. Allowed values are: draft, published.");
            }

            courses = await _unitOfWork.Courses.GetByStatusAsync(status);
        }

        var courseDtos = courses.Select(MapToDto);

        return ApiResponse<IEnumerable<CourseResponseDto>>.SuccessResponse(courseDtos, "Courses retrieved successfully");
    }

    public async Task<ApiResponse<CourseResponseDto>> CreateCourseAsync(CreateCourseRequestDto request)
    {
        var status = await _unitOfWork.Statuses.GetByNameAsync("Draft");
        if (status == null)
        {
            return ApiResponse<CourseResponseDto>.FailureResponse("The 'Draft' status does not exist in the database. Please check seeding.");
        }

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            StatusId = status.Id,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<CourseResponseDto>.SuccessResponse(MapToDto(course), "Course created successfully");
    }

    public async Task<ApiResponse<CourseResponseDto>> UpdateCourseAsync(Guid id, UpdateCourseRequestDto request)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            return ApiResponse<CourseResponseDto>.FailureResponse("Course not found.");
        }

        course.Title = request.Title;
        course.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Courses.UpdateAsync(course);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<CourseResponseDto>.SuccessResponse(MapToDto(course), "Course updated successfully");
    }

    public async Task<ApiResponse<bool>> DeleteCourseAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            return ApiResponse<bool>.FailureResponse("Course not found.");
        }

        course.IsDeleted = true;
        course.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Courses.UpdateAsync(course);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Course deleted successfully.");
    }

    public async Task<ApiResponse<CourseResponseDto>> PublishCourseAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            return ApiResponse<CourseResponseDto>.FailureResponse("Course not found.");
        }

        var hasLessons = await _unitOfWork.Lessons.HasActiveLessonsAsync(id);
        if (!hasLessons)
        {
            return ApiResponse<CourseResponseDto>.FailureResponse("Course must have at least one active lesson to be published.");
        }

        var status = await _unitOfWork.Statuses.GetByNameAsync("Published");
        if (status == null)
        {
            return ApiResponse<CourseResponseDto>.FailureResponse("Status 'Published' not found.");
        }

        course.StatusId = status.Id;
        course.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Courses.UpdateAsync(course);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<CourseResponseDto>.SuccessResponse(MapToDto(course), "Course published successfully.");
    }

    public async Task<ApiResponse<CourseResponseDto>> UnpublishCourseAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            return ApiResponse<CourseResponseDto>.FailureResponse("Course not found.");
        }

        var status = await _unitOfWork.Statuses.GetByNameAsync("Draft");
        if (status == null)
        {
            return ApiResponse<CourseResponseDto>.FailureResponse("Status 'Draft' not found.");
        }

        course.StatusId = status.Id;
        course.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Courses.UpdateAsync(course);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<CourseResponseDto>.SuccessResponse(MapToDto(course), "Course unpublished successfully.");
    }

    private CourseResponseDto MapToDto(Course course)
    {
        return new CourseResponseDto
        {
            Id = course.Id,
            Title = course.Title,
            StatusId = course.StatusId,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt
        };
    }
}
