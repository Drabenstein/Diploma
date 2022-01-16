using Application.Amazon;
using Application.Common;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Commands;
public static class DeleteThesis
{
    public record Command(int ThesisId) : IRequest<int>;

    public class Handler : IRequestHandler<Command, int>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IS3Service _s3Service;

        private const string GetCredentialsQuery = @" SELECT cloud_bucket AS Bucket, cloud_key AS Key
                    FROM thesis
                    WHERE thesis_id = :ThesisId
            ";

        private const string RemoveThesisCommand =
            @"UPDATE thesis
                SET (cloud_key, cloud_bucket) = (NULL, NULL)
                WHERE thesis_id = :ThesisId
            ";

        public Handler(ISqlConnectionFactory sqlConnectionFactory, IS3Service s3Service)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _s3Service = s3Service;
        }
        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);

            var credentials = await connection
                .QuerySingleAsync<ThesisCloudCredentialsDto>(GetCredentialsQuery, new
                {
                    request.ThesisId
                }).ConfigureAwait(false);

            var cloudKey = credentials.CloudKey;

            await _s3Service.RemoveThesisAsync(Utils.BUCKET_KEY, cloudKey).ConfigureAwait(false);

            var rowsAffected = await connection.ExecuteAsync(RemoveThesisCommand, new { request.ThesisId }).ConfigureAwait(false);
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
