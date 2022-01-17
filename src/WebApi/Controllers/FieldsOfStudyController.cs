using Application.Queries;
using Application.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/fieldsOfStudy")]
    public class FieldsOfStudyController : BaseApiController
    {
        private readonly IMediator _mediator;

        public FieldsOfStudyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns all fields of study
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>All fields of study</returns>
        /// <response code="200">Returns all fields of study</response>
        [HttpGet]
        public Task<IEnumerable<FieldOfStudyForApplicationDto>> GetAllFieldsOfStudy(CancellationToken cancellationToken)
        {
            return _mediator.Send(new GetAllFieldsOfStudy.Query(), cancellationToken);
        }
    }
}
