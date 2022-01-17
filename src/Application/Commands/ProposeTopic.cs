using Core.Models.Topics;
using Core.Models.Users;
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
            var user = _dbContext.Set<User>().FirstOrDefault(x => x.Email.Address == request.UserEmail);

            var fieldOfStudy = _dbContext.Set<FieldOfStudy>().FirstOrDefault(x => x.Id == request.FieldOfStudyId);

            var studentFieldOfStudy = _dbContext.Set<StudentFieldOfStudy>().FirstOrDefault(x => x.StudentId == user.Id && x.FieldOfStudyId == fieldOfStudy.Id);

            if (user == null || fieldOfStudy == null || studentFieldOfStudy == null) throw new InvalidOperationException("Application cannot be created with given data");

            var topic = new Topic
            {
                Proposer = user,
                IsProposedByStudent = true,
                MaxRealizationNumber = request.MaxRealizationNumber,
                IsAccepted = false,
                FieldOfStudy = fieldOfStudy,
                Name = request.PolishName,
                EnglishName = request.EnglishName,
                YearOfDefence = studentFieldOfStudy.PlannedYearOfDefence
            };

            await _dbContext.Set<Topic>().AddAsync(topic).ConfigureAwait(false);

            var application = new Core.Models.Topics.Application
            {
                Submitter = user as Student,
                Topic = topic,
                Timestamp = DateTime.Now,
                Message = request.Message,
                IsTopicProposal = true
            };

            await _dbContext.Set<Core.Models.Topics.Application>().AddAsync(application).ConfigureAwait(false);

            return Unit.Value;
        }
    }

}
