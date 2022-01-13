using Application.Queries;
using Application.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/theses")]
public class ThesesController : ControllerBase
{
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
    [Route("supervised")]
    public Task<IEnumerable<FieldOfStudyInitialTableDto<SupervisedThesisDto>>> GetSupervisedAsync()
    {
        // TODO: Get it from authenticated user info
        long tutorId = default;
        return _mediator.Send(new GetSupervisedTheses.Query(tutorId));
    }
}