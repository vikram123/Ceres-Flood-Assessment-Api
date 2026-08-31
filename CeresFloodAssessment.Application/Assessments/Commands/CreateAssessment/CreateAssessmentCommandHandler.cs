using CeresFloodAssessment.Application.Assessments.Dtos;
using CeresFloodAssessment.Application.Common.Interfaces;
using CeresFloodAssessment.Domain.Entities;
using CeresFloodAssessment.Domain.Enums;
using MediatR;

namespace CeresFloodAssessment.Application.Assessments.Commands.CreateAssessment;

public class CreateAssessmentCommandHandler : IRequestHandler<CreateAssessmentCommand, AssessmentDto>
{
    private readonly IAssessmentRepository _repository;
    private readonly IPhotoStorage _photoStorage;

    public CreateAssessmentCommandHandler(IAssessmentRepository repository, IPhotoStorage photoStorage)
    {
        _repository = repository;
        _photoStorage = photoStorage;
    }

    public async Task<AssessmentDto> Handle(CreateAssessmentCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
            throw new ArgumentException("Id is required.");
        if (string.IsNullOrWhiteSpace(request.Address))
            throw new ArgumentException("Address is required.");
        if (!Enum.TryParse<FarmCondition>(request.Condition, ignoreCase: true, out var condition))
            throw new ArgumentException($"Unknown condition '{request.Condition}'. Expected Good, Moderate, or Bad.");

        // Idempotency: a spotty connection can drop the response after the
        // server already committed the record, causing the client's
        // syncService to retry the same POST. Recognize the id we've
        // already synced and hand back that record instead of throwing a
        // duplicate-key error the client has no good way to recover from.
        var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (existing is not null)
        {
            return MapToDto(existing);
        }

        var capturedAt = DateTimeOffset.FromUnixTimeMilliseconds(request.CreatedAtUnixMs);

        var assessment = Assessment.Create(
            id: request.Id,
            address: request.Address,
            latitude: request.Latitude,
            longitude: request.Longitude,
            gpsAccuracy: null,
            condition: condition,
            chickenCount: request.ChickenCount,
            notes: request.Notes,
            capturedAt: capturedAt);

        foreach (var file in request.Photos)
        {
            if (file.Length == 0) continue;
            var stored = await _photoStorage.SaveAsync(assessment.Id, file, cancellationToken);
            assessment.AddPhoto(AssessmentPhoto.Create(assessment.Id, stored.FileName, stored.StoragePath, stored.SizeBytes));
        }

        await _repository.AddAsync(assessment, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapToDto(assessment);
    }

    private AssessmentDto MapToDto(Assessment a) => new(
        a.Id,
        a.Address,
        a.Latitude,
        a.Longitude,
        a.GpsAccuracy,
        a.Condition.ToString(),
        a.ChickenCount,
        a.Notes,
        a.CapturedAt,
        a.SyncedAt,
        a.Photos.Select(p => _photoStorage.ResolveUrl(p.StoragePath)).ToList()
    );
}
