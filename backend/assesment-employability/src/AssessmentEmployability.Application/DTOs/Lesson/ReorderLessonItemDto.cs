using System;
using System.ComponentModel.DataAnnotations;

namespace AssessmentEmployability.Application.DTOs.Lesson;

public class ReorderLessonItemDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public int Order { get; set; }
}
