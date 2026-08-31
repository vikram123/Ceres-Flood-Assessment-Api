namespace CeresFloodAssessment.Domain.Entities;

public class AssessmentPhoto
{
    public Guid Id { get; private set; }
    public string AssessmentId { get; private set; } = default!;
    public string FileName { get; private set; } = default!;

    /// <summary>Relative path (local disk) or blob URL (Azure), depending on the
    /// active IPhotoStorage implementation. The API resolves this to an
    /// absolute, browsable URL when mapping to AssessmentDto.</summary>
    public string StoragePath { get; private set; } = default!;

    public long SizeBytes { get; private set; }
    public DateTimeOffset UploadedAt { get; private set; }

    private AssessmentPhoto() { } // EF Core

    public static AssessmentPhoto Create(string assessmentId, string fileName, string storagePath, long sizeBytes)
    {
        return new AssessmentPhoto
        {
            Id = Guid.NewGuid(),
            AssessmentId = assessmentId,
            FileName = fileName,
            StoragePath = storagePath,
            SizeBytes = sizeBytes,
            UploadedAt = DateTimeOffset.UtcNow
        };
    }
}
