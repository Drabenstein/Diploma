using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;

namespace WebApi.Controllers;

[ApiController]
[Route("api/areasOfInterest")]
public class AreasOfInterestController : BaseApiController
{
    private readonly IMediator _mediator;
    public AreasOfInterestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns a collection of Areas of interest
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Collection of employees' Areas of interest</returns>
    /// <response code="200"></response>
    [HttpGet]
    [Route("getAreasOfInterest")]
    [Authorize(Roles = Role.ProgramCommittee + "," + Role.Tutor)]
    public Task<IEnumerable<AreaOfInterestDto>> GetAllAreasOfInterest(CancellationToken cancellationToken)
    {
        return _mediator.Send(new GetAllAreasOfInterest.Query(), cancellationToken);
    }

}
