using AssessmentEmployability.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssessmentEmployability.Application.Interfaces;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllAsync();
    Task<IEnumerable<Course>> GetByStatusAsync(string statusName);
    Task<Course?> GetByIdAsync(Guid id);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
}
