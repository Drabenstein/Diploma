using Application.Common;
using Application.Queries;
using Application.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;

namespace WebApi.Controllers;

[ApiController]
[Route("api/theses")]
public class ThesesController : BaseApiController
{
    private const long DefaultPage = 1L;
    private const long DefaultPageSize = 10L;
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
    [Authorize(Roles = "tutor")]
    public Task<IEnumerable<FieldOfStudyInitialTableDto<SupervisedThesisDto>>> GetSupervisedByFieldAsync(CancellationToken cancellationToken)
    {
        string userEmail = GetUserEmail();
        return _mediator.Send(new GetSupervisedTheses.Query(userEmail), cancellationToken);
    }

    /// <summary>
    /// Returns requested page of supervised theses
    /// </summary>
    /// <param name="fieldOfStudyId">Field of study id</param>
    /// <param name="yearOfDefence">Year of defence of the theses</param>
    /// <param name="page">Request result page, default: 1</param>
    /// <param name="pageSize">Count of items to return at max, default: 10</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Paged result with supervised theses</returns>
    /// <response code="200">Returns found paged supervised theses</response>
    [HttpGet]
    [Route("supervised")]
    [Authorize(Roles = "tutor")]
    public Task<PagedResultDto<SupervisedThesisDto>> GetSupervisedAsync(
        [FromQuery] long fieldOfStudyId, [FromQuery] string yearOfDefence, [FromQuery] long page = DefaultPage,
        [FromQuery] long pageSize = DefaultPageSize, CancellationToken cancellationToken = default)
    {
        string userEmail = GetUserEmail();
        return _mediator.Send(
            new GetSupervisedThesesForYearOfDefenceAndField.Query(userEmail, fieldOfStudyId, yearOfDefence, page,
                pageSize), cancellationToken);
    }
}