using Application.Amazon;
using FluentValidation;
using MediatR;

namespace Application.Queries;
public static class GetTranslatedThesisAbstract
{
    public record Query(string Text) : IRequest<string>;

    public class Handler : IRequestHandler<Query, string>
    {

        private readonly ITranslationService _translationService;

        public Handler(ITranslationService translationService)
        {
            _translationService = translationService;
        }
        //todo: to discuss - use comprehend to detect language and choose source language code?
        public async Task<string> Handle(Query query, CancellationToken cancellationToken)
        {
            var translatedText =
                await _translationService
                .TranslateTextAsync("pl", "en", query.Text)
                .ConfigureAwait(false);

            return translatedText;
        }
    
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Text).NotEmpty();
            }
        }

    }
}
