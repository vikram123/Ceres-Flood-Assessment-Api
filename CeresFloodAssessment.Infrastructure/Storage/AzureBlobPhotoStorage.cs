using Azure.Storage.Blobs;
using CeresFloodAssessment.Application.Common.Interfaces;
using CeresFloodAssessment.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CeresFloodAssessment.Infrastructure.Storage;

/// <summary>
/// Production storage target. Point Storage:Provider at "AzureBlob" and
/// supply Storage:AzureBlob:ConnectionString + ContainerName (pull the
/// connection string from Key Vault in real deployments, not appsettings).
/// </summary>
public class AzureBlobPhotoStorage : IPhotoStorage
{
    private readonly BlobContainerClient _containerClient;

    public AzureBlobPhotoStorage(IConfiguration configuration)
    {
        var connectionString = configuration["Storage:AzureBlob:ConnectionString"]
            ?? throw new InvalidOperationException("Storage:AzureBlob:ConnectionString is not configured.");
        var containerName = configuration["Storage:AzureBlob:ContainerName"] ?? "flood-assessment-photos";

        var serviceClient = new BlobServiceClient(connectionString);
        _containerClient = serviceClient.GetBlobContainerClient(containerName);
        _containerClient.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
    }

    public async Task<PhotoStorageResult> SaveAsync(string assessmentId, IFormFile file, CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(file.FileName);
        var blobName = $"{assessmentId}/{Guid.NewGuid():N}{extension}";
        var blobClient = _containerClient.GetBlobClient(blobName);

        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);

        return new PhotoStorageResult(file.FileName, blobName, file.Length);
    }

    public string ResolveUrl(string storagePath) => _containerClient.GetBlobClient(storagePath).Uri.ToString();
}
