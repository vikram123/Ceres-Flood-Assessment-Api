using CeresFloodAssessment.Application.Common.Interfaces;
using CeresFloodAssessment.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CeresFloodAssessment.Infrastructure.Storage;

/// <summary>
/// Zero-setup storage for local dev and the assignment demo: writes into
/// wwwroot/uploads/{assessmentId}/{filename} and serves it back via static
/// files. Swap to AzureBlobPhotoStorage for a real deployment — see
/// DependencyInjection.cs.
/// </summary>
public class LocalPhotoStorage : IPhotoStorage
{
    private readonly string _rootPath;
    private readonly string _publicBaseUrl;

    public LocalPhotoStorage(IConfiguration configuration)
    {
        _rootPath = configuration["Storage:LocalRootPath"] ?? Path.Combine(AppContext.BaseDirectory, "wwwroot", "uploads");
        _publicBaseUrl = (configuration["Storage:PublicBaseUrl"] ?? "/uploads").TrimEnd('/');
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<PhotoStorageResult> SaveAsync(string assessmentId, IFormFile file, CancellationToken cancellationToken)
    {
        var safeAssessmentId = SanitizeSegment(assessmentId);
        var directory = Path.Combine(_rootPath, safeAssessmentId);
        Directory.CreateDirectory(directory);

        var extension = Path.GetExtension(file.FileName);
        var storedFileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(directory, storedFileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        // StoragePath is relative — portable across machines/environments.
        var relativePath = $"{safeAssessmentId}/{storedFileName}";
        return new PhotoStorageResult(file.FileName, relativePath, file.Length);
    }

    public string ResolveUrl(string storagePath) => $"{_publicBaseUrl}/{storagePath}";

    private static string SanitizeSegment(string value) =>
        string.Concat(value.Where(c => char.IsLetterOrDigit(c) || c is '_' or '-'));
}
