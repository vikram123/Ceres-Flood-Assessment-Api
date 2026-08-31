using CeresFloodAssessment.Application.Assessments.Dtos;
using CeresFloodAssessment.Application.Common.Interfaces;
using MediatR;

namespace CeresFloodAssessment.Application.Assessments.Queries.GetAssessmentById;

public class GetAssessmentByIdQueryHandler : IRequestHandler<GetAssessmentByIdQuery, AssessmentDto?>
{
    private readonly IAssessmentRepository _repository;
    private readonly IPhotoStorage _photoStorage;

    public GetAssessmentByIdQueryHandler(IAssessmentRepository repository, IPhotoStorage photoStorage)
    {
        _repository = repository;
        _photoStorage = photoStorage;
    }

    public async Task<AssessmentDto?> Handle(GetAssessmentByIdQuery request, CancellationToken cancellationToken)
    {
        var a = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (a is null) return null;

        return new AssessmentDto(
            a.Id, a.Address, a.Latitude, a.Longitude, a.GpsAccuracy,
            a.Condition.ToString(), a.ChickenCount, a.Notes, a.CapturedAt, a.SyncedAt,
            a.Photos.Select(p => _photoStorage.ResolveUrl(p.StoragePath)).ToList());
    }
}
