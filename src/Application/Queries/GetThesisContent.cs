using Application.Amazon;
using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;
public static class GetThesisContent
{
    public record Query(int ThesisId) : IRequest<MemoryStream>;

    public class Handler : IRequestHandler<Query, MemoryStream>
    {
        private readonly IS3Service _s3Service;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        private const string CloudCredentialsQuery =
            @" SELECT cloud_bucket AS Bucket, cloud_key AS Key
                    FROM thesis
                    WHERE thesis_id = : ThesisId
            ";
        public Handler(IS3Service s3Service, ISqlConnectionFactory sqlConnectionFactory)
        {
            _s3Service = s3Service;
            _sqlConnectionFactory = sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<MemoryStream> Handle(Query query, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var credentials = await connection
                .QuerySingleAsync<ThesisCloudCredentialsDto>(CloudCredentialsQuery, new
                {
                    query.ThesisId
                }).ConfigureAwait(false);

            var fileResponse = await _s3Service
                .GetThesisAsync(credentials.CloudBucket, credentials.CloudKey)
                .ConfigureAwait(false);

            var memoryStream = new MemoryStream();
            await fileResponse.CopyToAsync(memoryStream).ConfigureAwait(false);
            return memoryStream;
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.ThesisId).GreaterThan(0);
        }
    }

}

