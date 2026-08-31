using CeresFloodAssessment.Domain.Entities;
using CeresFloodAssessment.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CeresFloodAssessment.Infrastructure.Persistence.Configurations;

public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment> builder)
    {
        builder.ToTable("Assessments");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasMaxLength(64)
            .ValueGeneratedNever(); // client-generated id — see Assessment.cs

        builder.Property(a => a.Address).IsRequired().HasMaxLength(500);
        builder.Property(a => a.Condition)
            .HasConversion<string>()
            .HasMaxLength(16);
        builder.Property(a => a.Notes).HasMaxLength(2000);

        builder.HasIndex(a => a.CapturedAt);
        builder.HasIndex(a => a.Condition);

        builder.HasMany(a => a.Photos)
            .WithOne()
            .HasForeignKey(p => p.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(a => a.Photos).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
