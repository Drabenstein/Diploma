using Application.ExternalServices;
using FluentValidation;
using MediatR;

namespace Application.Commands;

public static class ChangePassword
{
    public record Command(string UserExternalId, string Password) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IUserService _userService;

        public Handler(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            await _userService.ChangePasswordAsync(request.UserExternalId, request.Password).ConfigureAwait(false);
            return Unit.Value;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.UserExternalId).NotEmpty();
        }
    }
}
