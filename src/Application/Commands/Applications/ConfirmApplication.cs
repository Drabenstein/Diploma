using Core.Models.Theses;
using Core.Models.Topics.ValueObjects;
using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CoreTopics = Core.Models.Topics;

namespace Application.Commands.Applications;

public static class ConfirmApplication
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
                .Include(x => x.FieldOfStudy)
                .Include(x => x.Theses)
                .Include(x => x.Applications)
                .ThenInclude(a => a.Submitter)
                .Include(x => x.Supervisor)
                .SingleOrDefaultAsync(x => x.Applications.Any(a =>
                        a.Id == request.ApplicationId && a.Submitter.Email == request.StudentEmail),
                    cancellationToken: cancellationToken).ConfigureAwait(false);

            if (topic is null)
            {
                return false;
            }

            var applicationToConfirm = topic.Applications.First(x => x.Id == request.ApplicationId);
            var student = await _dbContext.Set<Student>()
                .Include(x => x.Applications)
                .ThenInclude(x => x.Topic)
                .ThenInclude(x => x.FieldOfStudy)
                .SingleAsync(x => x.Email == request.StudentEmail, cancellationToken).ConfigureAwait(false);

            if (student.Applications.Any(a =>
                    a.Id != applicationToConfirm.Id && a.Topic.YearOfDefence == topic.YearOfDefence &&
                    a.Topic.FieldOfStudy.Id == topic.FieldOfStudy.Id && a.Status == ApplicationStatus.Confirmed))
            {
                throw new InvalidOperationException(
                    "Student already confirmed another application for this field of study");
            }

            topic.ConfirmApplication(applicationToConfirm.Id);

            var thesis = topic.Theses.Last();

            thesis.ChangeReviewer(topic.Supervisor);

            await _dbContext.Set<Thesis>().AddAsync(thesis).ConfigureAwait(false);

            foreach (var application in student.Applications.Where(x => x.Id != applicationToConfirm.Id 
            && x.Topic.YearOfDefence == topic.YearOfDefence && x.Topic.FieldOfStudy.Id == topic.FieldOfStudy.Id 
            && (x.Status == ApplicationStatus.Sent || x.Status == ApplicationStatus.Approved)))
            {
                application.CancelApplication();
            }

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