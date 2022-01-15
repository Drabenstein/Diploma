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
        private readonly IComprehendService _comprehendService;

        public Handler(ITranslationService translationService, IComprehendService comprehendService)
        {
            _translationService = translationService;
            _comprehendService = comprehendService;
        }
        public async Task<string> Handle(Query query, CancellationToken cancellationToken)
        {
            var sourceLanguageCode = await _comprehendService
                .GetDominantLanguageAsync(query.Text)
                .ConfigureAwait(false);


            return await _translationService
                .TranslateTextAsync(sourceLanguageCode, "en", query.Text)
                .ConfigureAwait(false);
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
