using System.ComponentModel.DataAnnotations;

namespace AssessmentEmployability.Application.DTOs.Course;

public class CreateCourseRequestDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;
}
