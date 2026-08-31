using CeresFloodAssessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CeresFloodAssessment.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<AssessmentPhoto> Photos => Set<AssessmentPhoto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
