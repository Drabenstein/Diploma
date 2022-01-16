using Application.Commands;
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
}