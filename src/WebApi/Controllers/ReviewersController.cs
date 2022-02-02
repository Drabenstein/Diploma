using Application.Commands;
using Application.Commands.Dtos;
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
    [Authorize(Roles = Role.ProgramCommittee)]
    public Task<PagedResultDto<ReviewerWithInterestsDto>> GetReviewers([FromQuery] long[] AreaOfInterestIds, 
        [FromQuery] int minNoReviews = DefaultMinNoReviews, [FromQuery] int maxNoReviews = DefaultMaxNoReviews, [FromQuery] int page = DefaultPage,
        [FromQuery] int pageSize = DefaultPageSize, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(new GetReviewersWithInterests.Query(minNoReviews, maxNoReviews, AreaOfInterestIds, page, pageSize), cancellationToken);
    }

    /// <summary>
    /// Returns all theses assigned whose reviewer is the requesting user
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>All assigned theses grouped by field of study and year of defence</returns>
    /// <response code="200">Returns all assigned theses grouped by field of study and year of defence</response>
    [HttpGet]
    [Route("getMyReviews")]
    [Authorize(Roles = Role.Tutor)]
    public Task<IEnumerable<FieldOfStudyInitialTableDto<ReviewersThesisDto>>> GetThesesForReview(CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new GetReviewersTheses.Query(email), cancellationToken);
    }


    /// <summary>
    /// Returns requested page of theses whose reviewer is the requesting user
    /// </summary>
    /// <param name="fieldOfStudyId">Field of study id</param>
    /// <param name="yearOfDefence">Year of defence of the theses</param>
    /// <param name="page">Request result page, default: 1</param>
    /// <param name="pageSize">Count of items to return at max, default: 10</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Paged result with theses whose reviewer is the requesting user</returns>
    /// <response code="200">Found theses whose reviewer is the requesting user</response>
    [HttpGet]
    [Route("getMyReviewsPage")]
    [Authorize(Roles = Role.Tutor)]
    public Task<PagedResultDto<ReviewersThesisDto>> GetTopicsForTutorPage(
        [FromQuery] long fieldOfStudyId, [FromQuery] string yearOfDefence, [FromQuery] int page = DefaultPage,
        [FromQuery] int pageSize = DefaultPageSize, CancellationToken cancellationToken = default)
    {
        string userEmail = GetUserEmail();
        return _mediator.Send(
            new GetReviewersThesesForFieldOfStudyAndYear.Query(userEmail, fieldOfStudyId, yearOfDefence, page,
                pageSize), cancellationToken);
    }

    /// <summary>
    /// Returns initial data for review
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="ThesisId">Id of thesis to review</param>
    /// <returns>DTO with initial data for review</returns>
    /// <response code="200">Returns DTO with initial data for review</response>
    [HttpGet]
    [Route("getDataForReview")]
    [Authorize(Roles = Role.Tutor)]
    public Task<ReviewDataDto> GetDataForReview([FromQuery] long ThesisId, CancellationToken cancellationToken)
    {
        string userEmail = GetUserEmail();
        return _mediator.Send(new GetDataForReview.Query(userEmail, ThesisId), cancellationToken);
    }

    /// <summary>
    /// Posts tutor's thesis review 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="ReviewId">Id of the thesis review</param>
    /// <response code="200">Post successful</response>
    [HttpPost]
    [Route("postReview")]
    [Authorize(Roles = Role.Tutor)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public Task PostReview([FromBody] SubmitReviewDto dto, CancellationToken cancellationToken)
    {
        string userEmail = GetUserEmail();
        return _mediator.Send(new SubmitReview.Command(userEmail, dto.ReviewModules, dto.ReviewId, dto.Grade), cancellationToken);
    }

}
