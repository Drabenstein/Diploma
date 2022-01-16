using Application.Common;
using Core;
using Core.Models.Topics.ValueObjects;
using Core.Models.Users;
using Dapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
                .SingleOrDefaultAsync(x => x.Applications.Any(a =>
                        a.Id == request.ApplicationId && a.Submitter.Email.Address == request.StudentEmail),
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
                .SingleAsync(x => x.Email.Address == request.StudentEmail, cancellationToken).ConfigureAwait(false);

            if (student.Applications.Any(a =>
                    a.Id != applicationToConfirm.Id && a.Topic.YearOfDefence == topic.YearOfDefence &&
                    a.Topic.FieldOfStudy.Id == topic.FieldOfStudy.Id && a.Status == ApplicationStatus.Confirmed))
            {
                throw new InvalidOperationException(
                    "Student already confirmed another application for this field of study");
            }

            topic.ConfirmApplication(applicationToConfirm.Id);

            foreach (var topicToCancelApplication in student.Applications.Select(x => x.Topic))
            {
                var applicationToCancelId =
                    topicToCancelApplication.Applications.First(a => a.Submitter.Email.Address == request.StudentEmail)
                        .Id;
                topicToCancelApplication.CancelApplication(applicationToCancelId);
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