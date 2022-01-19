using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CoreTopics = Core.Models.Topics;

namespace Application.Commands.Applications;

public static class CancelApplication
{
    public record Command(string StudentEmail, long ApplicationId) : IRequest<bool>;

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var topic = await _dbContext.Set<CoreTopics.Topic>()
                .Include(x => x.Applications)
                .SingleOrDefaultAsync(
                    x => x.Applications.Any(a =>
                        a.Id == request.ApplicationId && a.Submitter.Email == request.StudentEmail),
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (topic is null)
            {
                return false;
            }

            topic.CancelApplication(request.ApplicationId);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.StudentEmail).EmailAddress();
            RuleFor(x => x.ApplicationId).GreaterThan(0);
        }
    }
}