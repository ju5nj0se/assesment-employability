using System;

namespace AssessmentEmployability.Application.DTOs.Course;

public class CourseResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int StatusId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
