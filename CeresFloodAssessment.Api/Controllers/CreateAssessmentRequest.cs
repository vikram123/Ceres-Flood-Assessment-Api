using Microsoft.AspNetCore.Mvc;

namespace CeresFloodAssessment.Api.Controllers;

/// <summary>
/// Binds the multipart/form-data fields exactly as syncService.js's
/// uploadOne() names them.
/// </summary>
public class CreateAssessmentRequest
{
    [FromForm(Name = "id")] public string Id { get; set; } = default!;
    [FromForm(Name = "address")] public string Address { get; set; } = default!;
    [FromForm(Name = "latitude")] public double? Latitude { get; set; }
    [FromForm(Name = "longitude")] public double? Longitude { get; set; }
    [FromForm(Name = "condition")] public string Condition { get; set; } = default!;
    [FromForm(Name = "chickenCount")] public int ChickenCount { get; set; }
    [FromForm(Name = "notes")] public string? Notes { get; set; }
    [FromForm(Name = "createdAt")] public long CreatedAt { get; set; }
    [FromForm(Name = "photos")] public List<IFormFile> Photos { get; set; } = new();
}
