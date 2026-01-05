using AssessmentEmployability.Application.Interfaces;
using AssessmentEmployability.Infrastructure.Data;
using AssessmentEmployability.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AssessmentEmployability.Infrastructure.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly AppDbContext _context;

    public StatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Statuses.AnyAsync(s => s.Id == id);
    }

    public async Task<Status?> GetByNameAsync(string name)
    {
        return await _context.Statuses
            .FirstOrDefaultAsync(s => s.StatusName.ToLower() == name.ToLower());
    }
}
