using System;
using System.ComponentModel.DataAnnotations;

namespace AssessmentEmployability.Application.DTOs.Lesson;

public class CreateLessonRequestDto
{
    [Required(ErrorMessage = "CourseId is required")]
    public Guid CourseId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;
}
