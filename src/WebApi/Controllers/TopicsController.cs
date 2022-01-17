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


}

