using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/aws")]
    public class AWSController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AWSController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("translate")]
        public Task<string> GetTranslatedText([FromQuery] string text, CancellationToken cancellationToken)
        {
            return _mediator.Send(new GetTranslatedThesisAbstract.Query(text), cancellationToken);
        }

        [HttpGet]
        [Route("downloadThesis")]
        public async Task<IActionResult> DownloadThesis([FromQuery] int thesisId, CancellationToken cancellationToken)
        {
            var thesisFileStream = await _mediator.Send(new GetThesisContent.Query(thesisId), cancellationToken);
            return File(thesisFileStream, "application/pdf");
        }

        [HttpPost]
        [RequestSizeLimit(20_000_000)]
        [Route("uploadThesis")]
        public async Task<IActionResult> UploadThesis([FromQuery] int thesisId, CancellationToken cancellationToken)
        {
            var file = Request.Form.Files[0];
            await _mediator.Send(new UploadThesis.Command(thesisId, file), cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("deleteThesis")]
        public async Task<IActionResult> DeleteThesis([FromQuery] int thesisId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteThesis.Command(thesisId), cancellationToken);
            return Ok();
        }
    }
}
