using CeresFloodAssessment.Application.Assessments.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CeresFloodAssessment.Application.Assessments.Commands.CreateAssessment;

/// <summary>
/// Mirrors exactly what the React app's syncService.js posts as
/// multipart/form-data to POST /api/assessments: id, address, latitude,
/// longitude, condition, chickenCount, notes, createdAt, and one or more
/// "photos" files.
/// </summary>
public record CreateAssessmentCommand(
    string Id,
    string Address,
    double? Latitude,
    double? Longitude,
    string Condition,
    int ChickenCount,
    string? Notes,
    long CreatedAtUnixMs,
    IReadOnlyList<IFormFile> Photos
) : IRequest<AssessmentDto>;
