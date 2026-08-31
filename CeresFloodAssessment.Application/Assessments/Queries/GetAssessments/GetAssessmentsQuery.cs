using CeresFloodAssessment.Application.Assessments.Dtos;
using MediatR;

namespace CeresFloodAssessment.Application.Assessments.Queries.GetAssessments;

public record GetAssessmentsQuery : IRequest<List<AssessmentDto>>;
