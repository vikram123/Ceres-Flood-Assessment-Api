using MediatR;

namespace CeresFloodAssessment.Application.Assessments.Commands.DeleteAssessment;

public record DeleteAssessmentCommand(string Id) : IRequest<bool>;
