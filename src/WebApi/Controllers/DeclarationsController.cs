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

    /// <summary>
    /// Requests sending a declaration by student
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="declarationDto">Data transfer object with declaration data</param>
    /// <response code="200"></response>
    [HttpPost]
    [Authorize(Roles = Role.StudentRole)]
    public Task SendDeclaration([FromBody] SendDeclarationDto declarationDto, CancellationToken cancellationToken)
    {
        return _mediator.Send(new SendDeclaration.Command(declarationDto), cancellationToken);
    }
}
