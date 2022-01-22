using Application.Commands;
using Application.Commands.Dtos;
using Application.Queries;
using Application.Queries.Dtos;
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
    [Authorize(Roles = Role.Student)]
    public Task SendDeclaration([FromBody] SendDeclarationDto declarationDto, CancellationToken cancellationToken)
    {
        return _mediator.Send(new SendDeclaration.Command(declarationDto), cancellationToken);
    }

    /// <summary>
    /// Returns a DTO with thesis data for declaration
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="ThesisId">Id of the thesis whose data will be retrieved</param>
    /// <returns>DTO with thesis data for declaration</returns>
    /// <response code="200">Returns a DTO with thesis data for declaration</response>
    [HttpGet]
    [Authorize(Roles = Role.Student)]
    public Task<DeclarationDataDto> GetDataForDeclaration([FromQuery] long ThesisId, CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new GetDataForDeclaration.Query(email, ThesisId), cancellationToken);
    }
}
