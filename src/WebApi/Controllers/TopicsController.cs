using Application.Commands;
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
    /// <returns>All topics grouped by field of study and year of defence</returns>
    /// <response code="200">Returns all topics grouped by field of study and year of defence</response>
    [HttpGet]
    [Authorize(Roles = Role.StudentRole + "," + Role.TutorRole)]
    public Task<IEnumerable<FieldOfStudyInitialTableDto<StudentsTopicDto>>> GetAllTopicsAsync(CancellationToken cancellationToken)
    {
        return _mediator.Send(new GetAllTopics.Query(), cancellationToken);
    }

    [HttpPost]
    [Authorize(Roles = Role.StudentRole)]
    public Task ProposeTopic([FromQuery] long TutorId, [FromQuery] long FieldOfStudyId,
        [FromQuery] int MaxRealizationNumber, [FromQuery] string PolishName, [FromQuery] string EnglishName,
        [FromQuery] string Message, CancellationToken cancellationToken)
    {
        string email = GetUserEmail();
        return _mediator.Send(new ProposeTopic.Command(email, TutorId, FieldOfStudyId, MaxRealizationNumber, PolishName, EnglishName, Message), cancellationToken);
    }
    

}

