namespace CeresFloodAssessment.Application.Assessments.Dtos;

public record AssessmentDto(
    string Id,
    string Address,
    double? Latitude,
    double? Longitude,
    double? GpsAccuracy,
    string Condition,
    int ChickenCount,
    string? Notes,
    DateTimeOffset CapturedAt,
    DateTimeOffset SyncedAt,
    List<string> PhotoUrls
);
