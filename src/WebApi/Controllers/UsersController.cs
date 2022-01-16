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
    public async Task<IActionResult> FetchDataAsync(CancellationToken cancellationToken )
    {
        string email = GetUserEmail();
        var roles = GetUserRoles();
        var result = await _mediator.Send(new FetchUserData.Command(email, roles), cancellationToken);
        return result ? Ok() : NoContent();
    }

    [HttpGet]
    [Route("myData/student")]
    [Authorize(Roles = Role.StudentRole)]
    public Task<IEnumerable<StudentDataDto>> GetStudentData(CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new GetStudentData.Query(email), cancellationToken);
    }

    [HttpGet]
    [Route("myData/tutor")]
    [Authorize(Roles = Role.TutorRole)]
    public Task<TutorDataDto> GetStudGetTutorDataentData(CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new GetTutorData.Query(email), cancellationToken);
    }

    [HttpPost]
    [Route("myData/updateAreasOfInterest")]
    [Authorize(Roles = Role.TutorRole)]
    public async Task<IActionResult> UpdateAreasOfInterest([FromBody] long[] AreasOfInterestIds, CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        await _mediator.Send(new UpdateTutorAreasOfInterest.Command(AreasOfInterestIds, email), cancellationToken);
        return Ok();
    }
}