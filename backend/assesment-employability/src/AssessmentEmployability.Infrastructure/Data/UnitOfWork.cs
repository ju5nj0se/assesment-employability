using AssessmentEmployability.Application.Interfaces;
using AssessmentEmployability.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace AssessmentEmployability.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private ICourseRepository? _courses;
    private ILessonRepository? _lessons;
    private IStatusRepository? _statuses;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ICourseRepository Courses => _courses ??= new CourseRepository(_context);
    public ILessonRepository Lessons => _lessons ??= new LessonRepository(_context);
    public IStatusRepository Statuses => _statuses ??= new StatusRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
