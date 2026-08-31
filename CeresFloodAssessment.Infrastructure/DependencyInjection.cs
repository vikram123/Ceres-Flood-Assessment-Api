using CeresFloodAssessment.Application.Common.Interfaces;
using CeresFloodAssessment.Infrastructure.Persistence;
using CeresFloodAssessment.Infrastructure.Persistence.Repositories;
using CeresFloodAssessment.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CeresFloodAssessment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var dbProvider = configuration["Database:Provider"] ?? "Sqlite";

        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=ceres-flood-assessment.db";

            if (dbProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
                options.UseSqlServer(connectionString);
            else
                options.UseSqlite(connectionString);
        });

        services.AddScoped<IAssessmentRepository, AssessmentRepository>();

        var storageProvider = configuration["Storage:Provider"] ?? "Local";
        if (storageProvider.Equals("AzureBlob", StringComparison.OrdinalIgnoreCase))
            services.AddSingleton<IPhotoStorage, AzureBlobPhotoStorage>();
        else
            services.AddSingleton<IPhotoStorage, LocalPhotoStorage>();

        return services;
    }
}
