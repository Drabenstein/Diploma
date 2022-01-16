using Application.Common;
using Application.Queries;
using Application.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;

namespace WebApi.Controllers;

[ApiController]
[Route("api/reviewers")]
public class ReviewersController : BaseApiController
{
    private readonly IMediator _mediator;
    public ReviewersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("getReviewersWithInterests")]
    [Authorize(Roles = Role.DeansAssistantRole)]
    public Task<PagedResultDto<ReviewerWithInterestsDto>> GetReviewers([FromQuery] int minNoReviews, 
        [FromQuery] int maxNoReviews, [FromQuery] long[] AreaOfInterestIds, [FromQuery] int page = DefaultPage,
        [FromQuery] int pageSize = DefaultPageSize, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(new GetReviewersWithInterests.Query(minNoReviews, maxNoReviews, AreaOfInterestIds, page, pageSize), cancellationToken);
    }

}
