using AssessmentEmployability.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssessmentEmployability.Application.Interfaces;

public interface ILessonRepository
{
    Task<bool> HasActiveLessonsAsync(Guid courseId);
    Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId);
    Task<Lesson?> GetByIdAsync(Guid id);
    Task<int> GetMaxOrderAsync(Guid courseId);
    Task AddAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task UpdateRangeAsync(IEnumerable<Lesson> lessons);
}
