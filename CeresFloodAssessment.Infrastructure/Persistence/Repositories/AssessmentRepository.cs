using CeresFloodAssessment.Application.Common.Interfaces;
using CeresFloodAssessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CeresFloodAssessment.Infrastructure.Persistence.Repositories;

public class AssessmentRepository : IAssessmentRepository
{
    private readonly AppDbContext _db;

    public AssessmentRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Assessment?> GetByIdAsync(string id, CancellationToken cancellationToken) =>
        _db.Assessments.Include(a => a.Photos).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public Task<List<Assessment>> GetAllAsync(CancellationToken cancellationToken) =>
        _db.Assessments.Include(a => a.Photos).AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(Assessment assessment, CancellationToken cancellationToken) =>
        await _db.Assessments.AddAsync(assessment, cancellationToken);

    public void Remove(Assessment assessment) => _db.Assessments.Remove(assessment);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        _db.SaveChangesAsync(cancellationToken);
}
