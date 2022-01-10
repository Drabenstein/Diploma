namespace Application.Amazon
{
    public interface ITranslationService
    {
        Task<string> TranslateTextAsync(string sourceLanguageCode, string targetLanguageCode, string text);
    }
}
