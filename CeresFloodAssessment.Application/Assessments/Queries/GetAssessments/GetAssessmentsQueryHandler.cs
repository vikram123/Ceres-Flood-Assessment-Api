using CeresFloodAssessment.Application.Assessments.Dtos;
using CeresFloodAssessment.Application.Common.Interfaces;
using CeresFloodAssessment.Domain.Entities;
using MediatR;

namespace CeresFloodAssessment.Application.Assessments.Queries.GetAssessments;

public class GetAssessmentsQueryHandler : IRequestHandler<GetAssessmentsQuery, List<AssessmentDto>>
{
    private readonly IAssessmentRepository _repository;
    private readonly IPhotoStorage _photoStorage;

    public GetAssessmentsQueryHandler(IAssessmentRepository repository, IPhotoStorage photoStorage)
    {
        _repository = repository;
        _photoStorage = photoStorage;
    }

    public async Task<List<AssessmentDto>> Handle(GetAssessmentsQuery request, CancellationToken cancellationToken)
    {
        var assessments = await _repository.GetAllAsync(cancellationToken);
        return assessments
            .OrderByDescending(a => a.CapturedAt)
            .Select(Map)
            .ToList();
    }

    private AssessmentDto Map(Assessment a) => new(
        a.Id, a.Address, a.Latitude, a.Longitude, a.GpsAccuracy,
        a.Condition.ToString(), a.ChickenCount, a.Notes, a.CapturedAt, a.SyncedAt,
        a.Photos.Select(p => _photoStorage.ResolveUrl(p.StoragePath)).ToList());
}
