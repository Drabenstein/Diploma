using Core.Models.Topics;
using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;
public static class ProposeTopic
{
    public record Command(string UserEmail, long TutorId, long FieldOfStudyId, int MaxRealizationNumber, string PolishName, string EnglishName, string Message) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = _dbContext.Set<Student>().FirstOrDefault(x => x.Email.Address == request.UserEmail);

            var fieldOfStudy = await _dbContext.Set<FieldOfStudy>().FirstOrDefaultAsync(x => x.Id == request.FieldOfStudyId).ConfigureAwait(false);

            var studentFieldOfStudy = await _dbContext.Set<StudentFieldOfStudy>().FirstOrDefaultAsync(x => x.StudentId == user.Id && x.FieldOfStudyId == fieldOfStudy.Id).ConfigureAwait(false);

            if (user == null || fieldOfStudy == null || studentFieldOfStudy == null) throw new InvalidOperationException("Application cannot be created with given data");

            var topic = new Topic
            {
                Proposer = user,
                IsProposedByStudent = true,
                MaxRealizationNumber = request.MaxRealizationNumber,
                FieldOfStudy = fieldOfStudy,
                Name = request.PolishName,
                EnglishName = request.EnglishName,
                YearOfDefence = studentFieldOfStudy.PlannedYearOfDefence
            };

            await _dbContext.Set<Topic>().AddAsync(topic, cancellationToken).ConfigureAwait(false);

            var application = new Core.Models.Topics.Application
            {
                Submitter = user,
                Topic = topic,
                Timestamp = DateTime.Now,
                Message = request.Message,
                IsTopicProposal = true
            };

            await _dbContext.Set<Core.Models.Topics.Application>().AddAsync(application, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Unit.Value;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.EnglishName).NotEmpty();
            RuleFor(x => x.Message).NotEmpty();
            RuleFor(x => x.UserEmail).EmailAddress();
            RuleFor(x => x.MaxRealizationNumber).GreaterThanOrEqualTo(0);
            RuleFor(x => x.FieldOfStudyId).GreaterThan(0);
            RuleFor(x => x.PolishName).NotEmpty();
        }
    }

}
