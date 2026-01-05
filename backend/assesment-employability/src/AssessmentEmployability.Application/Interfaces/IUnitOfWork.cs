using System;
using System.Threading.Tasks;

namespace AssessmentEmployability.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICourseRepository Courses { get; }
    ILessonRepository Lessons { get; }
    IStatusRepository Statuses { get; }
    Task<int> SaveChangesAsync();
}
