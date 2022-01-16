using Application.Common;
using Application.Queries;
using Application.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/applications")]
public class ApplicationsController : BaseApiController
{
    private readonly IMediator _mediator;

    public ApplicationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns all field of studies where user has open applications
    /// </summary>
    /// <returns>Field of studies with paged applications</returns>
    /// <response code="200">Returns found field of studies with paged applications</response>
    [HttpGet]
    [Route("initial-by-field")]
    [Authorize(Roles = Role.TutorRole)]
    public Task<IEnumerable<FieldOfStudyInitialTableDto<ApplicationDto>>> GetApplicationsInitialTableAsync(CancellationToken cancellationToken)
    {
        string userEmail = GetUserEmail();
        return _mediator.Send(new GetApplications.Query(userEmail), cancellationToken);
    }

    /// <summary>
    /// Returns requested page of applications
    /// </summary>
    /// <param name="fieldOfStudyId">Field of study id</param>
    /// <param name="yearOfDefence">Year of defence of the theses</param>
    /// <param name="page">Request result page, default: 1</param>
    /// <param name="pageSize">Count of items to return at max, default: 10</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Paged result with applications</returns>
    /// <response code="200">Returns found paged applications</response>
    [HttpGet]
    [Authorize(Roles = Role.TutorRole)]
    public Task<PagedResultDto<ApplicationDto>> GetSupervisedAsync(
        [FromQuery] long fieldOfStudyId, [FromQuery] string yearOfDefence, [FromQuery] int page = DefaultPage,
        [FromQuery] int pageSize = DefaultPageSize, CancellationToken cancellationToken = default)
    {
        string userEmail = GetUserEmail();
        return _mediator.Send(
            new GetApplicationForYearOfDefenceAndField.Query(userEmail, fieldOfStudyId, yearOfDefence, page,
                pageSize), cancellationToken);
    }
}