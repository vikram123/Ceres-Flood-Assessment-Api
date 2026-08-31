using CeresFloodAssessment.Application.Assessments.Commands.CreateAssessment;
using CeresFloodAssessment.Application.Assessments.Commands.DeleteAssessment;
using CeresFloodAssessment.Application.Assessments.Dtos;
using CeresFloodAssessment.Application.Assessments.Queries.GetAssessmentById;
using CeresFloodAssessment.Application.Assessments.Queries.GetAssessments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CeresFloodAssessment.Api.Controllers;

[ApiController]
[Route("api/assessments")]
public class AssessmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AssessmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Receives one synced site assessment from the field app, including
    /// any photos taken at the site. Called by the frontend's
    /// syncService.js once the assessor's device is back online.
    /// </summary>
    [HttpPost]
    [RequestSizeLimit(200_000_000)] // several full-res photos per site
    [ProducesResponseType(typeof(AssessmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AssessmentDto>> Create([FromForm] CreateAssessmentRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateAssessmentCommand(
            request.Id,
            request.Address,
            request.Latitude,
            request.Longitude,
            request.Condition,
            request.ChickenCount,
            request.Notes,
            request.CreatedAt,
            request.Photos);

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<AssessmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AssessmentDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAssessmentsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AssessmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssessmentDto>> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAssessmentByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteAssessmentCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
