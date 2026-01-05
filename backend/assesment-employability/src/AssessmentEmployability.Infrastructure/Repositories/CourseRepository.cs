using AssessmentEmployability.Application.Interfaces;
using AssessmentEmployability.Domain.Entities;
using AssessmentEmployability.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentEmployability.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses
            .Where(c => !c.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetByStatusAsync(string statusName)
    {
        return await _context.Courses
            .Join(_context.Statuses,
                course => course.StatusId,
                status => status.Id,
                (course, status) => new { Course = course, Status = status })
            .Where(x => !x.Course.IsDeleted && x.Status.StatusName.ToLower() == statusName.ToLower())
            .Select(x => x.Course)
            .ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
    }
}
