using AssessmentEmployability.Application.Common;
using AssessmentEmployability.Application.DTOs.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssessmentEmployability.Application.Interfaces;

public interface ICourseService
{
    Task<ApiResponse<IEnumerable<CourseResponseDto>>> GetCoursesAsync(string? status);
    Task<ApiResponse<CourseResponseDto>> CreateCourseAsync(CreateCourseRequestDto request);
    Task<ApiResponse<CourseResponseDto>> UpdateCourseAsync(Guid id, UpdateCourseRequestDto request);
    Task<ApiResponse<bool>> DeleteCourseAsync(Guid id);
    Task<ApiResponse<CourseResponseDto>> PublishCourseAsync(Guid id);
    Task<ApiResponse<CourseResponseDto>> UnpublishCourseAsync(Guid id);
}
