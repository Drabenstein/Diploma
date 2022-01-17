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

    [HttpGet]
    [Authorize(Roles = Role.StudentRole + "," + Role.TutorRole)]
    public Task<IEnumerable<FieldOfStudyInitialTableDto<StudentsTopicDto>>> GetAllTopicsAsync(CancellationToken cancellationToken)
    {
        return _mediator.Send(new GetAllTopics.Query(), cancellationToken);
    }
    

}

