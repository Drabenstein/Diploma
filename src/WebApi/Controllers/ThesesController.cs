using Application.Common;
using Application.Queries;
using Application.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/theses")]
public class ThesesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ThesesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns all field of studies where user supervises at least one thesis with initial number of theses
    /// </summary>
    /// <returns>Field of studies with paged supervised theses</returns>
    /// <response code="200">Returns found field of studies with paged supervised theses</response>
    [HttpGet]
    [Route("supervisedByField")]
    public Task<IEnumerable<FieldOfStudyInitialTableDto<SupervisedThesisDto>>> GetSupervisedByFieldAsync(CancellationToken cancellationToken)
    {
        // TODO: Get it from authenticated user info
        long tutorId = default;
        return _mediator.Send(new GetSupervisedTheses.Query(tutorId), cancellationToken);
    }

    /// <summary>
    /// Returns requested page of supervised theses
    /// </summary>
    /// <param name="fieldOfStudyId">Field of study id</param>
    /// <param name="yearOfDefence">Year of defence of the theses</param>
    /// <param name="page">Request result page</param>
    /// <param name="pageSize">Count of items to return at max</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Paged result with supervised theses</returns>
    /// <response code="200">Returns found paged supervised theses</response>
    [HttpGet]
    [Route("supervised")]
    public Task<PagedResultDto<SupervisedThesisDto>> GetSupervisedAsync(
        [FromQuery] int fieldOfStudyId, [FromQuery] string yearOfDefence, [FromQuery] int page,
        [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        // TODO: Get it from authenticated user info
        long tutorId = default;
        return _mediator.Send(
            new GetSupervisedThesesForYearOfDefenceAndField.Query(tutorId, fieldOfStudyId, yearOfDefence, page,
                pageSize), cancellationToken);
    }
}