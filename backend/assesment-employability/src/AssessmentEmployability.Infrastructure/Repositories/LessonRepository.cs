using AssessmentEmployability.Application.Interfaces;
using AssessmentEmployability.Infrastructure.Data;
using AssessmentEmployability.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentEmployability.Infrastructure.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _context;

    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasActiveLessonsAsync(Guid courseId)
    {
        return await _context.Lessons
            .AnyAsync(l => l.CourseId == courseId && !l.IsDeleted);
    }

    public async Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId)
    {
        return await _context.Lessons
            .Where(l => l.CourseId == courseId && !l.IsDeleted)
            .OrderBy(l => l.Order)
            .ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(Guid id)
    {
        return await _context.Lessons
            .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted);
    }

    public async Task<int> GetMaxOrderAsync(Guid courseId)
    {
        var maxOrder = await _context.Lessons
            .Where(l => l.CourseId == courseId && !l.IsDeleted)
            .Select(l => (int?)l.Order)
            .MaxAsync();
        
        return maxOrder ?? 0;
    }

    public async Task AddAsync(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
    }

    public async Task UpdateRangeAsync(IEnumerable<Lesson> lessons)
    {
        _context.Lessons.UpdateRange(lessons);
    }
}
