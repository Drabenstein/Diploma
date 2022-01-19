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
[Route("api/topics")]
public class TopicsController : BaseApiController
{
    private readonly IMediator _mediator;

    public TopicsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns all topics grouped by field of study and year of defence
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>All topics grouped by field of study and year of defence</returns>
    /// <response code="200">Returns all topics grouped by field of study and year of defence</response>
    [HttpGet]
    [Authorize(Roles = Role.StudentRole + "," + Role.TutorRole)]
    public Task<IEnumerable<FieldOfStudyInitialTableDto<StudentsTopicDto>>> GetAllTopicsAsync(CancellationToken cancellationToken)
    {
        return _mediator.Send(new GetAllTopics.Query(), cancellationToken);
    }

    /// <summary>
    /// Returns all topics grouped by field of study and year of defence
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>All topics grouped by field of study and year of defence</returns>
    /// <response code="200">Returns all topics grouped by field of study and year of defence</response>
    [HttpGet]
    [Route("topicsForTutor")]
    [Authorize(Roles = Role.TutorRole)]
    public Task<IEnumerable<FieldOfStudyInitialTableDto<TutorsTopicDto>>> GetTutorsTopics(CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new GetAllTopicsForTutor.Query(email), cancellationToken);
    }

    /// <summary>
    /// Returns requested page of topics
    /// </summary>
    /// <param name="fieldOfStudyId">Field of study id</param>
    /// <param name="yearOfDefence">Year of defence of the theses</param>
    /// <param name="page">Request result page, default: 1</param>
    /// <param name="pageSize">Count of items to return at max, default: 10</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Paged result with topics</returns>
    /// <response code="200">Found topics</response>
    [HttpGet]
    [Route("topicsPage")]
    [Authorize(Roles = Role.StudentRole + "," + Role.TutorRole)]
    public Task<PagedResultDto<StudentsTopicDto>> GetTopicsPage(
        [FromQuery] long fieldOfStudyId, [FromQuery] string yearOfDefence, [FromQuery] int page = DefaultPage,
        [FromQuery] int pageSize = DefaultPageSize, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(
            new GetAllTopicsForFieldOfStudyAndYear.Query(fieldOfStudyId, yearOfDefence, page,
                pageSize), cancellationToken);
    }


    /// <summary>
    /// Returns requested page of topics for tutor
    /// </summary>
    /// <param name="fieldOfStudyId">Field of study id</param>
    /// <param name="yearOfDefence">Year of defence of the theses</param>
    /// <param name="page">Request result page, default: 1</param>
    /// <param name="pageSize">Count of items to return at max, default: 10</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Paged result with topics for tutor</returns>
    /// <response code="200">Found topics for tutor</response>
    [HttpGet]
    [Route("topicsForTutorPage")]
    [Authorize(Roles = Role.TutorRole)]
    public Task<PagedResultDto<TutorsTopicDto>> GetTopicsForTutorPage(
        [FromQuery] long fieldOfStudyId, [FromQuery] string yearOfDefence, [FromQuery] int page = DefaultPage,
        [FromQuery] int pageSize = DefaultPageSize, CancellationToken cancellationToken = default)
    {
        string userEmail = GetUserEmail();
        return _mediator.Send(
            new GetAllTopicsForTutorForFieldOfStudyAndYear.Query(userEmail, fieldOfStudyId, yearOfDefence, page,
                pageSize), cancellationToken);
    }

    /// <summary>
    /// Sends a new topic application
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="TutorId">UserId of the tutor who is chosen to be the supervisor</param>
    /// <param name="FieldOfStudyId">ID of the requesting student's field of study</param>
    /// <param name="EnglishName">Name of the topic in english</param>
    /// <param name="MaxRealizationNumber">Maximum number of students who can work on this topic in their thesis</param>
    /// <param name="Message">Message to the requested topic supervisor</param>
    /// <param name="PolishName">Name of the topic in polish</param>
    /// <response code="200">Application was added</response>
    [HttpPost]
    [Authorize(Roles = Role.StudentRole)]
    public Task ProposeTopic([FromQuery] long TutorId, [FromQuery] long FieldOfStudyId,
        [FromQuery] int MaxRealizationNumber, [FromQuery] string PolishName, [FromQuery] string EnglishName,
        [FromQuery] string Message, CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new ProposeTopic.Command(email, TutorId, FieldOfStudyId, MaxRealizationNumber, PolishName, EnglishName, Message), cancellationToken);
    }

    /// <summary>
    /// Returns a list of possible thesis supervisors
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Returns a list of possible thesis supervisors</response>
    [HttpGet]
    [Route("possibleTutors")]
    [Authorize(Roles = Role.StudentRole)]
    public Task<IEnumerable<TutorForApplicationDto>> GetTutorsForApplication(CancellationToken cancellationToken)
    {
        return _mediator.Send(new GetTutorsForApplication.Query(), cancellationToken);
    }

    /// <summary>
    /// Returns a list of current student's fields of study
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Returns a list of current student's fields of study</response>
    [HttpGet]
    [Route("possibleFieldsOfStudy")]
    [Authorize(Roles = Role.StudentRole)]
    public Task<IEnumerable<FieldOfStudyForApplicationDto>> GetFieldsOfStudyForApplication(CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new GetFieldsOfStudyForApplication.Query(email), cancellationToken);
    }

    /// <summary>
    /// Sends an application for an existing topic
    /// </summary>
    /// <param name="Message">Message to topic supervisor</param>
    /// <param name="TopicId">Id of the topic applied for</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Application was added</response>
    [HttpPost]
    [Route("applyForTopic")]
    [Authorize(Roles = Role.StudentRole)]
    public Task ApplyForTopic([FromQuery] long TopicId, string Message, CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new ApplyForTopic.Command(email, TopicId, Message), cancellationToken);
    }

    /// <summary>
    /// Creates a new topic
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="dto">DTO with topic's name in en and pl, max number of realizations,
    /// year of defence and field of study</param>
    /// <response code="200">Topic was added</response>
    [HttpPost]
    [Route("createTopic")]
    [Authorize(Roles = Role.TutorRole)]
    public Task CreateTopic([FromBody] CreateTopicDto dto, CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new CreateTopic.Command(email, dto.FieldOfStudyId, dto.YearOfDefence,
            dto.MaxNoRealizations, dto.PolishName, dto.EnglishName), cancellationToken);
    }


    /// <summary>
    /// Returns student's approved topics
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>A DTO with topic and application ids, topic name in pl and en, supervisor data and approved status</returns>
    /// <response code="200">Student's approved topics</response>
    [HttpGet]
    [Route("myAcceptedTopics")]
    [Authorize(Roles = Role.StudentRole)]
    public Task<IEnumerable<ApprovedTopicDto>> GetMyApprovedTopics(CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new GetMyApprovedTopics.Query(email), cancellationToken);
    }
    
    /// <summary>
    /// Returns all field of studies with paged topics which were not considered
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("for-consideration-initial")]
    [Authorize(Roles = Role.ProgramCommittee)]
    [ProducesResponseType(typeof(IEnumerable<FieldOfStudyInitialTableDto<TopicForConsiderationDto>>),
        StatusCodes.Status200OK)]
    public Task<IEnumerable<FieldOfStudyInitialTableDto<TopicForConsiderationDto>>>
        GetTopicsForConsiderationInitialAsync(CancellationToken cancellationToken)
    {
        return _mediator.Send(new GetTopicsForConsideration.Query(), cancellationToken);
    }

    /// <summary>
    /// Gets unconsidered topics page for field of study and year of defence
    /// </summary>
    /// <param name="fieldOfStudyId">Field of study id</param>
    /// <param name="yearOfDefence">Year of defence</param>
    /// <param name="page">Requested page</param>
    /// <param name="pageSize">Max items to return per page</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("for-consideration")]
    [Authorize(Roles = Role.ProgramCommittee)]
    [ProducesResponseType(typeof(PagedResultDto<TopicForConsiderationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public Task<PagedResultDto<TopicForConsiderationDto>> GetTopicsForConsiderationAsync(
        [FromQuery] long fieldOfStudyId, [FromQuery] string yearOfDefence, [FromQuery] int page = DefaultPage,
        [FromQuery] int pageSize = DefaultPageSize, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(
            new GetTopicsForConsiderationByYearOfDefenceAndField.Query(fieldOfStudyId, yearOfDefence, page, pageSize),
            cancellationToken);
    }
    
    /// <summary>
    /// Accepts topics in bulk
    /// </summary>
    /// <param name="topicsIds">Topics ids to accept, max: 50</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("bulk-accept")]
    [Authorize(Roles = Role.ProgramCommittee)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> BulkAcceptTopicsAsync([FromBody] long[]? topicsIds, CancellationToken cancellationToken)
    {
        if (topicsIds is null)
        {
            return BadRequest();
        }

        await _mediator.Send(new BulkAcceptTopics.Command(topicsIds), cancellationToken);
        return Ok();
    }
    
    /// <summary>
    /// Rejects topics in bulk
    /// </summary>
    /// <param name="topicsIds">Topics ids to reject, max: 50</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("bulk-reject")]
    [Authorize(Roles = Role.ProgramCommittee)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> BulkRejectTopicsAsync([FromBody] long[]? topicsIds, CancellationToken cancellationToken)
    {
        if (topicsIds is null)
        {
            return BadRequest();
        }

        await _mediator.Send(new BulkRejectTopics.Command(topicsIds), cancellationToken);
        return Ok();
    }
}

