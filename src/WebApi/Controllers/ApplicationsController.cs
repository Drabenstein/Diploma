using Application.Commands.Applications;
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
    public Task<IEnumerable<FieldOfStudyInitialTableDto<ApplicationDto>>> GetApplicationsInitialTableAsync(
        CancellationToken cancellationToken)
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
    /// <response code="409">Action cannot be permitted</response>
    /// <response code="422">Validation of the request failed</response>
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

    /// <summary>
    /// Accepts specified application
    /// </summary>
    /// <param name="applicationId">Application id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Application accepted successfully</response>
    /// <response code="404">Related topic not found or topic not supervised by logged-in user</response>
    /// <response code="409">Action cannot be permitted</response>
    /// <response code="422">Validation of the request failed</response>
    [HttpPost]
    [Route("{applicationId}/accept")]
    [Authorize(Roles = "tutor")]
    public async Task<IActionResult> AcceptAsync([FromRoute] long applicationId, CancellationToken cancellationToken)
    {
        var email = GetUserEmail();
        var result = await _mediator.Send(new AcceptApplication.Command(email, applicationId), cancellationToken);
        return result ? Ok() : NotFound();
    }

    /// <summary>
    /// Rejects specified application
    /// </summary>
    /// <param name="applicationId">Application id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Application rejected successfully</response>
    /// <response code="404">Related topic not found or topic not supervised by logged-in user</response>
    /// <response code="409">Action cannot be permitted</response>
    /// <response code="422">Validation of the request failed</response>
    [HttpPost]
    [Route("{applicationId}/reject")]
    [Authorize(Roles = "tutor")]
    public async Task<IActionResult> RejectAsync([FromRoute] long applicationId, CancellationToken cancellationToken)
    {
        var email = GetUserEmail();
        var result = await _mediator.Send(new RejectApplication.Command(email, applicationId), cancellationToken);
        return result ? Ok() : NotFound();
    }

    /// <summary>
    /// Confirms specified application
    /// </summary>
    /// <param name="applicationId">Application id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Application confirmed successfully</response>
    /// <response code="404">Related topic not found or application not submitted by logged-in user</response>
    /// <response code="409">Action cannot be permitted</response>
    /// <response code="422">Validation of the request failed</response>
    [HttpPost]
    [Route("{applicationId}/confirm")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> ConfirmAsync([FromRoute] long applicationId, CancellationToken cancellationToken)
    {
        var email = GetUserEmail();
        var result = await _mediator.Send(new AcceptApplication.Command(email, applicationId), cancellationToken);
        return result ? Ok() : NotFound();
    }

    /// <summary>
    /// Cancels specified application
    /// </summary>
    /// <param name="applicationId">Application id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Application cancelled successfully</response>
    /// <response code="404">Related topic not found or application not submitted by logged-in user</response>
    /// <response code="409">Action cannot be permitted</response>
    /// <response code="422">Validation of the request failed</response>
    [HttpPost]
    [Route("{applicationId}/cancel")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> CancelAsync([FromRoute] long applicationId, CancellationToken cancellationToken)
    {
        var email = GetUserEmail();
        var result = await _mediator.Send(new AcceptApplication.Command(email, applicationId), cancellationToken);
        return result ? Ok() : NotFound();
    }

    /// <summary>
    /// Get details of application by id
    /// </summary>
    /// <param name="applicationId">Application id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Found application details</returns>
    /// <response code="200">Application found</response>
    /// <response code="404">Application not found</response>
    /// <response code="409">Action cannot be permitted</response>
    /// <response code="422">Validation of the request failed</response>
    [HttpGet]
    [Route("{applicationId}")]
    [Authorize(Roles = "tutor,student")]
    public async Task<IActionResult> GetDetailsAsync([FromRoute] long applicationId,
        CancellationToken cancellationToken)
    {
        var email = GetUserEmail();
        var result = await _mediator.Send(new GetApplicationDetailsById.Query(email, applicationId), cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }
}
