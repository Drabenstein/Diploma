using System.Text;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Common;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private const string InvalidRequestHeaderMessage = "Invalid request due to: ";
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        ValidationFailure[] errors = _validators.Select(validator => validator.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error is not null)
            .ToArray();

        if (errors.Length == 0)
        {
            return next();
        }

        var errorBuilder = new StringBuilder();
        errorBuilder.AppendLine(InvalidRequestHeaderMessage);

        foreach (ValidationFailure error in errors)
        {
            errorBuilder.AppendLine(error.ErrorMessage);
        }

        throw new ValidationException(errorBuilder.ToString());
    }
}