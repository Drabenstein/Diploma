using Application.Commands.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;
public static class SendDeclaration
{
    public record Command(SendDeclarationDto declarationDto) : IRequest<Unit>;
    
    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        private const string Command = @"
                INSERT INTO declaration (objective_of_work, operating_range, language, date, thesis_id)
                VALUES (:ObjectiveOfWork, :OperatingRange, :Language, :Date, :ThesisId)
                ";

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);

            var date = request.declarationDto.DeclarationDateTime.Date.ToString("yyyy-MM-dd");

            await connection.ExecuteAsync(Command, new
            {
                ObjectiveOfWork = request.declarationDto.ObjectiveOfWork,
                OperatingRange = request.declarationDto.OperatingRange,
                Language = request.declarationDto.Language,
                Date = date,
                ThesisId = request.declarationDto.ThesisId
            }).ConfigureAwait(false);

            return new Unit();
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.declarationDto.Language).NotEmpty();
            RuleFor(x => x.declarationDto.ThesisId).GreaterThan(0);
            RuleFor(x => x.declarationDto.DeclarationDateTime.Date).LessThanOrEqualTo(DateTime.Now.Date);
            RuleFor(x => x.declarationDto.OperatingRange).NotEmpty();
            RuleFor(x => x.declarationDto.ObjectiveOfWork).NotEmpty();
        }
    }
}
