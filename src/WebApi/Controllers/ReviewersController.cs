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
    private const int DefaultMinNoReviews = 0;
    private const int DefaultMaxNoReviews = 50;
    public ReviewersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns requested page reviewers and their interests
    /// </summary>
    /// <param name="AreaOfInterestIds">Array with ids of chosen areas of interest</param>
    /// <param name="minNoReviews">Minimum number of reviews for a reviewer to be selected</param>
    /// <param name="maxNoReviews">Maximum number of reviews for a reviewer to be selected</param>
    /// <param name="page">Request result page, default: 1</param>
    /// <param name="pageSize">Count of items to return at max, default: 10</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Paged result with reviewers and their areas of interest</returns>
    /// <response code="200">Returns requested page reviewers and their interests</response>
    [HttpGet]
    [Route("getReviewersWithInterests")]
    [Authorize(Roles = Role.DeansAssistantRole)]
    public Task<PagedResultDto<ReviewerWithInterestsDto>> GetReviewers([FromQuery] long[] AreaOfInterestIds, 
        [FromQuery] int minNoReviews = DefaultMinNoReviews, [FromQuery] int maxNoReviews = DefaultMaxNoReviews, [FromQuery] int page = DefaultPage,
        [FromQuery] int pageSize = DefaultPageSize, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(new GetReviewersWithInterests.Query(minNoReviews, maxNoReviews, AreaOfInterestIds, page, pageSize), cancellationToken);
    }
}
