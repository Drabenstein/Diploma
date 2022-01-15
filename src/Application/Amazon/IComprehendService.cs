namespace Application.Amazon
{
    public interface IComprehendService
    {
        Task<int> FilterWordInTextAsync(string text, string languageCode, string word);
        Task<string> GetDominantLanguageAsync(string text);
        Task<string> GetSentimentAsync(string text, string languageCode);
    }
}
