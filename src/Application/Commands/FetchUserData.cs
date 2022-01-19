using System.Diagnostics.CodeAnalysis;
using Application.ExternalServices;
using Core.Models.Users;
using Core.Models.Users.ValueObjects;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Commands;

public static class FetchUserData
{
    public record Command(string Email, IReadOnlyCollection<string> Roles) : IRequest<bool>;

    public class Handler : IRequestHandler<Command, bool>
    {
        private const string CacheKeyBase = nameof(FetchUserData);
        private readonly IUserDataFetcher _userDataFetcher;
        private readonly DbContext _dbContext;
        private readonly IMemoryCache _cache;

        public Handler(IUserDataFetcher userDataFetcher, DbContext dbContext, IMemoryCache cache)
        {
            _userDataFetcher = userDataFetcher;
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{CacheKeyBase}-{request.Email}";
            if (_cache.Get<User>(cacheKey) is not null)
            {
                return false;
            }

            var allUsers = await _dbContext.Set<User>().ToListAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            var user = allUsers.FirstOrDefault(x =>
                string.Equals(x.Email.Address, request.Email, StringComparison.OrdinalIgnoreCase));

            if (user is null)
            {
                var userData = await _userDataFetcher.FetchUserDataAsync(request.Email).ConfigureAwait(false);
                user = CreateUser(userData, request.Roles);
                await _dbContext.Set<User>().AddAsync(user, cancellationToken).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            _cache.Set(cacheKey, user);
            return true;
        }

        [SuppressMessage("Usage", "MA0011:IFormatProvider is missing")]
        private User CreateUser(UserDataDto userDataDto, IReadOnlyCollection<string> roles)
        {
            var (firstName, lastName, email) = userDataDto;
            if (roles.Any(x => string.Equals(x, "tutor", StringComparison.OrdinalIgnoreCase)))
            {
                return new Tutor(firstName, lastName, new Email(email));
            }

            if (roles.Any(x => string.Equals(x, "student", StringComparison.OrdinalIgnoreCase)))
            {
                return new Student(firstName, lastName, email, 0);
            }

            return new User(firstName, lastName, new Email(email));
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Roles).NotEmpty();
        }
    }
}