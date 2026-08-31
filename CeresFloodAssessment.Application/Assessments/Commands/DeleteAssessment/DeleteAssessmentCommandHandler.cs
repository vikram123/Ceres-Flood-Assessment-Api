using CeresFloodAssessment.Application.Common.Interfaces;
using MediatR;

namespace CeresFloodAssessment.Application.Assessments.Commands.DeleteAssessment;

public class DeleteAssessmentCommandHandler : IRequestHandler<DeleteAssessmentCommand, bool>
{
    private readonly IAssessmentRepository _repository;

    public DeleteAssessmentCommandHandler(IAssessmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteAssessmentCommand request, CancellationToken cancellationToken)
    {
        var assessment = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (assessment is null) return false;

        _repository.Remove(assessment);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
