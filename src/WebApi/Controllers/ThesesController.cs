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
    private readonly IMediator _mediator;

    public ThesesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns id of user's thesis
    /// </summary>
    /// <returns>Id of user's thesis. 0 if user has no thesis</returns>
    /// <response code="200">Returns id of user's thesis</response>
    [HttpGet]
    [Route("get-thesis-id")]
    [Authorize(Roles = Role.StudentRole)]
    public Task<long> GetUsersThesisId(CancellationToken cancellationToken)
    {
        string userEmail = GetUserEmail();
        return _mediator.Send(new GetUserThesisId.Query(userEmail), cancellationToken);
    }

    /// <summary>
    /// Returns user's thesis by thesis id
    /// </summary>
    /// <returns>User's thesis data and it's reviews</returns>
    /// <response code="200">Returns user's thesis data and it's reviews</response>
    [HttpGet]
    [Route("my-thesis")]
    [Authorize(Roles = Role.StudentRole)]
    public Task<MyThesisDto> GetMyThesis([FromQuery] long ThesisId, CancellationToken cancellationToken)
    {
        string userEmail = GetUserEmail();
        return _mediator.Send(new GetMyThesis.Query(userEmail, ThesisId), cancellationToken);
    }

    /// <summary>
    /// Returns all field of studies where user supervises at least one thesis with initial number of theses
    /// </summary>
    /// <returns>Field of studies with paged supervised theses</returns>
    /// <response code="200">Returns found field of studies with paged supervised theses</response>
    [HttpGet]
    [Route("supervised-by-field")]
    [Authorize(Roles = Role.TutorRole)]
    public Task<IEnumerable<FieldOfStudyInitialTableDto<SupervisedThesisDto>>> GetSupervisedByFieldAsync(
        CancellationToken cancellationToken)
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
    /// <response code="200">Found supervised theses</response>
    /// <response code="422">Invalid parameters have been passed to request</response>
    [HttpGet]
    [Route("supervised")]
    [Authorize(Roles = Role.TutorRole)]
    public Task<PagedResultDto<SupervisedThesisDto>> GetSupervisedAsync(
        [FromQuery] long fieldOfStudyId, [FromQuery] string yearOfDefence, [FromQuery] int page = DefaultPage,
        [FromQuery] int pageSize = DefaultPageSize, CancellationToken cancellationToken = default)
    {
        string userEmail = GetUserEmail();
        return _mediator.Send(
            new GetSupervisedThesesForYearOfDefenceAndField.Query(userEmail, fieldOfStudyId, yearOfDefence, page,
                pageSize), cancellationToken);
    }

    /// <summary>
    /// Gets fields of studies with paged theses which are open to assign reviewer
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("reviewer-assignment-initial")]
    [Authorize(Roles = Role.ProgramCommittee)]
    [ProducesResponseType(typeof(IEnumerable<FieldOfStudyInitialTableDto<ThesisForReviewerAssignmentDto>>), StatusCodes.Status200OK)]
    public Task<IEnumerable<FieldOfStudyInitialTableDto<ThesisForReviewerAssignmentDto>>>
        GetThesesForReviewerAssignmentByFieldAsync(CancellationToken cancellationToken)
    {
        return _mediator.Send(new GetThesesForReviewerAssignment.Query(), cancellationToken);
    }

    /// <summary>
    /// Gets page of theses open to assign a reviewer for specific field of study id and year of defence
    /// </summary>
    /// <param name="fieldOfStudyId">Field of study id</param>
    /// <param name="yearOfDefence">Year of defence</param>
    /// <param name="page">Page</param>
    /// <param name="pageSize">Maximum items per page</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("reviewer-assignment")]
    [Authorize(Roles = Role.ProgramCommittee)]
    [ProducesResponseType(typeof(PagedResultDto<ThesisForReviewerAssignmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public Task<PagedResultDto<ThesisForReviewerAssignmentDto>>
        GetThesesForReviewerAssignmentAsync([FromQuery] long fieldOfStudyId, [FromQuery] string yearOfDefence,
            [FromQuery] int page = DefaultPage,
            [FromQuery] int pageSize = DefaultPageSize, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(
            new GetThesesForReviewerAssignmentForYearOfDefenceAndField.Query(fieldOfStudyId, yearOfDefence, page,
                pageSize), cancellationToken);
    }
}