using Application.Amazon;
using Core;
using Dapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Application.Commands;
public static class UploadThesis
{
    public record Command(int ThesisId, IFormFile File) : IRequest<int>;

    public class Handler : IRequestHandler<Command, int>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IS3Service _s3Service;
        public Handler(ISqlConnectionFactory sqlConnectionFactory, IS3Service s3Service)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _s3Service = s3Service;
        }
        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var file = request.File;

            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName!.Trim('"');
            var tempPath = Path.Combine(Path.GetTempPath(), fileName);
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var fileContent = File.ReadAllBytes(tempPath);
            var cloudKey = Guid.NewGuid().ToString();

            await _s3Service.UploadThesisAsync(Utils.BUCKET_KEY, cloudKey, fileContent).ConfigureAwait(false);

            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);

            string UploadThesisCommand =
            @$"UPDATE thesis
                SET (cloud_key, cloud_bucket) = ('{cloudKey}','{Utils.BUCKET_KEY}')
                WHERE thesis_id = {request.ThesisId}
            ";

            var rowsAffected = await connection.ExecuteAsync(UploadThesisCommand).ConfigureAwait(false);
            return rowsAffected;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ThesisId).GreaterThan(0);
        }
    }
}
