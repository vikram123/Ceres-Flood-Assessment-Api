using CeresFloodAssessment.Domain.Enums;

namespace CeresFloodAssessment.Domain.Entities;

/// <summary>
/// A single site visit record. The Id is generated client-side (see the
/// React app's db.js: generateId()) rather than server-side, because
/// records are created offline, on the device, before the server has ever
/// heard of them. Using the client id as the primary key is also what makes
/// re-sync idempotent: if a retry re-POSTs the same assessment after a
/// dropped connection, the server recognizes it as the same record instead
/// of creating a duplicate.
/// </summary>
public class Assessment
{
    public string Id { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }
    public double? GpsAccuracy { get; private set; }
    public FarmCondition Condition { get; private set; }
    public int ChickenCount { get; private set; }
    public string? Notes { get; private set; }

    /// <summary>When the assessor captured this on their device (client clock).</summary>
    public DateTimeOffset CapturedAt { get; private set; }

    /// <summary>When the server first received this record.</summary>
    public DateTimeOffset SyncedAt { get; private set; }

    private readonly List<AssessmentPhoto> _photos = new();
    public IReadOnlyCollection<AssessmentPhoto> Photos => _photos.AsReadOnly();

    private Assessment() { } // EF Core

    public static Assessment Create(
        string id,
        string address,
        double? latitude,
        double? longitude,
        double? gpsAccuracy,
        FarmCondition condition,
        int chickenCount,
        string? notes,
        DateTimeOffset capturedAt)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Id is required.", nameof(id));
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address is required.", nameof(address));
        if (chickenCount < 0)
            throw new ArgumentException("Chicken count cannot be negative.", nameof(chickenCount));

        return new Assessment
        {
            Id = id,
            Address = address.Trim(),
            Latitude = latitude,
            Longitude = longitude,
            GpsAccuracy = gpsAccuracy,
            Condition = condition,
            ChickenCount = chickenCount,
            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim(),
            CapturedAt = capturedAt,
            SyncedAt = DateTimeOffset.UtcNow
        };
    }

    public void AddPhoto(AssessmentPhoto photo) => _photos.Add(photo);
}
