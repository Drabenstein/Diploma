using Application.Commands;
using Application.Queries;
using Application.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/users")]
public class UsersController : BaseApiController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("fetchData")]
    [Authorize]
    public async Task<IActionResult> FetchDataAsync(CancellationToken cancellationToken = default)
    {
        string email = GetUserEmail();
        var roles = GetUserRoles();
        var result = await _mediator.Send(new FetchUserData.Command(email, roles), cancellationToken);
        return result ? Ok() : NoContent();
    }

    /// <summary>
    /// Returns student's personal data
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>StudentDataDto object with student's personal data</returns>
    /// <response code="200">Returns student's personal data</response>
    [HttpGet]
    [Route("myData/student")]
    [Authorize(Roles = Role.Student)]
    public Task<IEnumerable<StudentDataDto>> GetStudentData(CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new GetStudentData.Query(email), cancellationToken);
    }

    /// <summary>
    /// Returns tutor's personal data
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>TutorDataDto object with tutor's personal data</returns>
    /// <response code="200">Returns tutor's personal data</response>
    [HttpGet]
    [Route("myData/tutor")]
    [Authorize(Roles = Role.Tutor)]
    public Task<TutorDataDto> GetStudGetTutorDataentData(CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new GetTutorData.Query(email), cancellationToken);
    }

    /// <summary>
    /// Updates tutor's areas of interest
    /// </summary>
    /// <param name="AreasOfInterestIds">Array consisting of all (previous and new) area of interest ids, which should be connected to requesting tutor</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpPost]
    [Route("myData/updateAreasOfInterest")]
    [Authorize(Roles = Role.Tutor)]
    public async Task<IActionResult> UpdateAreasOfInterest([FromBody] long[] AreasOfInterestIds, CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        await _mediator.Send(new UpdateTutorAreasOfInterest.Command(AreasOfInterestIds, email), cancellationToken);
        return Ok();
    }
}