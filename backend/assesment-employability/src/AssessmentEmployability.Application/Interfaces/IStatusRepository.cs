using AssessmentEmployability.Domain.Entities;
using System.Threading.Tasks;

namespace AssessmentEmployability.Application.Interfaces;

public interface IStatusRepository
{
    Task<bool> ExistsAsync(int id);
    Task<Status?> GetByNameAsync(string name);
}
