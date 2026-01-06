using Moq;
using AssessmentEmployability.Application.Services;
using AssessmentEmployability.Application.Interfaces;
using AssessmentEmployability.Application.DTOs.Course;
using AssessmentEmployability.Domain.Entities;
using AssessmentEmployability.Application.Common;
using Xunit;

namespace AssessmentEmployability.UnitTests;

public class CourseServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _courseService = new CourseService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task CreateCourse_ReturnsSuccess_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateCourseRequestDto { Title = "Test Course" };
        var status = new Status { Id = Guid.NewGuid(), Name = "Draft" };
        
        _mockUnitOfWork.Setup(u => u.Statuses.GetByNameAsync("Draft"))
            .ReturnsAsync(status);
        _mockUnitOfWork.Setup(u => u.Courses.AddAsync(It.IsAny<Course>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _courseService.CreateCourseAsync(request);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Test Course", result.Data.Title);
        Assert.Equal(status.Id, result.Data.StatusId);
    }

    [Fact]
    public async Task PublishCourse_ReturnsFailure_WhenNoLessonsExist()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Course { Id = courseId, Title = "Empty Course" };
        
        _mockUnitOfWork.Setup(u => u.Courses.GetByIdAsync(courseId))
            .ReturnsAsync(course);
        _mockUnitOfWork.Setup(u => u.Lessons.HasActiveLessonsAsync(courseId))
            .ReturnsAsync(false);

        // Act
        var result = await _courseService.PublishCourseAsync(courseId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Course must have at least one active lesson to be published.", result.Message);
    }

    [Fact]
    public async Task PublishCourse_ReturnsSuccess_WhenLessonsExist()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Course { Id = courseId, Title = "Ready Course", StatusId = Guid.NewGuid() };
        var publishedStatus = new Status { Id = Guid.NewGuid(), Name = "Published" };
        
        _mockUnitOfWork.Setup(u => u.Courses.GetByIdAsync(courseId))
            .ReturnsAsync(course);
        _mockUnitOfWork.Setup(u => u.Lessons.HasActiveLessonsAsync(courseId))
            .ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.Statuses.GetByNameAsync("Published"))
            .ReturnsAsync(publishedStatus);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _courseService.PublishCourseAsync(courseId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(publishedStatus.Id, result.Data.StatusId);
    }

    [Fact]
    public async Task GetCourses_ReturnsFailure_WhenInvalidStatusProvided()
    {
        // Act
        var result = await _courseService.GetCoursesAsync("invalid_status");

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Invalid status filter", result.Message);
    }

    [Fact]
    public async Task UpdateCourse_ReturnsFailure_WhenCourseNotFound()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        _mockUnitOfWork.Setup(u => u.Courses.GetByIdAsync(courseId))
            .ReturnsAsync((Course)null);

        // Act
        var result = await _courseService.UpdateCourseAsync(courseId, new UpdateCourseRequestDto { Title = "New Title" });

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Course not found.", result.Message);
    }
}
