using CeresFloodAssessment.Domain.Entities;

namespace CeresFloodAssessment.Application.Common.Interfaces;

public interface IAssessmentRepository
{
    Task<Assessment?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<List<Assessment>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Assessment assessment, CancellationToken cancellationToken);
    void Remove(Assessment assessment);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
