using Application.Commands;
using Application.Commands.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;

namespace WebApi.Controllers;

[ApiController]
[Route("api/declarations")]
public class DeclarationsController : BaseApiController
{
    private readonly IMediator _mediator;
    public DeclarationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("sendDeclaration")]
    [Authorize(Roles = Role.StudentRole)]
    public Task SendDeclaration([FromBody] SendDeclarationDto declarationDto, CancellationToken cancellationToken)
    {
        return _mediator.Send(new SendDeclaration.Command(declarationDto), cancellationToken);
    }
}
