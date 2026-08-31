using CeresFloodAssessment.Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace CeresFloodAssessment.Application.Common.Interfaces;

/// <summary>
/// Abstraction over where uploaded photos land. Two implementations ship in
/// Infrastructure: local disk (zero setup, fine for the assignment/demo)
/// and Azure Blob Storage (production — matches the rest of the Azure
/// footprint: APIM, AKS, Key Vault). Swapping is a single config value,
/// see Infrastructure/DependencyInjection.cs.
/// </summary>
public interface IPhotoStorage
{
    Task<PhotoStorageResult> SaveAsync(string assessmentId, IFormFile file, CancellationToken cancellationToken);

    /// <summary>Resolves a stored path/key into a URL the frontend can load in an &lt;img&gt;.</summary>
    string ResolveUrl(string storagePath);
}
