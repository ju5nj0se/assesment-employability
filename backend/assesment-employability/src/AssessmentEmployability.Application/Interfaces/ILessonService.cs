using AssessmentEmployability.Application.Common;
using AssessmentEmployability.Application.DTOs.Lesson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssessmentEmployability.Application.Interfaces;

public interface ILessonService
{
    Task<ApiResponse<IEnumerable<LessonResponseDto>>> GetLessonsByCourseIdAsync(Guid courseId);
    Task<ApiResponse<LessonResponseDto>> CreateLessonAsync(CreateLessonRequestDto request);
    Task<ApiResponse<LessonResponseDto>> UpdateLessonAsync(Guid id, UpdateLessonRequestDto request);
    Task<ApiResponse<bool>> DeleteLessonAsync(Guid id);
    Task<ApiResponse<bool>> ReorderLessonsAsync(List<ReorderLessonItemDto> items);
}
