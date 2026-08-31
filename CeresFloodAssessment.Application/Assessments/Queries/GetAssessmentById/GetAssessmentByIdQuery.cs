using CeresFloodAssessment.Application.Assessments.Dtos;
using MediatR;

namespace CeresFloodAssessment.Application.Assessments.Queries.GetAssessmentById;

public record GetAssessmentByIdQuery(string Id) : IRequest<AssessmentDto?>;
