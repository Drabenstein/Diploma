using Core.Models.Topics;
using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public static class CreateTopic
{
    public record Command(string UserEmail, long FieldOfStudyId, int YearOfDefence, int MaxNoRealizations, string PolishName, string EnglishName) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var users = await _dbContext.Set<Tutor>().ToListAsync();
            var user = users.Where(x => x.Email.Address == request.UserEmail).FirstOrDefault();


            var fieldOfStudy = await _dbContext.Set<FieldOfStudy>().FirstOrDefaultAsync(x => x.Id == request.FieldOfStudyId).ConfigureAwait(false);
            
            if(user == null || fieldOfStudy == null) throw new InvalidOperationException("Topic cannot be created with given data");

            var topic = new Topic
            {
                Proposer = user,
                Supervisor = user,
                Name = request.PolishName,
                EnglishName = request.EnglishName,
                IsFree = true,
                MaxRealizationNumber = request.MaxNoRealizations,
                YearOfDefence = request.YearOfDefence.ToString(),
                FieldOfStudy = fieldOfStudy
            };

            await _dbContext.Set<Topic>().AddAsync(topic, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }


    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.EnglishName).NotEmpty();
            RuleFor(x => x.UserEmail).EmailAddress();
            RuleFor(x => x.YearOfDefence).GreaterThanOrEqualTo(DateTime.Now.Year);
            RuleFor(x => x.MaxNoRealizations).GreaterThan(0);
            RuleFor(x => x.FieldOfStudyId).GreaterThan(0);
            RuleFor(x => x.PolishName).NotEmpty();
        }
    }

}
