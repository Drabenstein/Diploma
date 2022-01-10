using Amazon.Translate;
using Amazon.Translate.Model;
using Application.Amazon;
using Core.Amazon;

namespace Infrastructure.AWS
{
    public class TranslationService : ITranslationService
    {
        private readonly AmazonTranslateClient _amazonTranslateClient;
        public TranslationService(AmazonTranslateClient amazonTranslateClient)
        {
            _amazonTranslateClient = amazonTranslateClient;
        }

        public async Task<string> TranslateTextAsync(string sourceLanguageCode, string targetLanguageCode, string text)
        {
            try
            {
                var request = new TranslateTextRequest
                {
                    Text = text,
                    SourceLanguageCode = sourceLanguageCode,
                    TargetLanguageCode = targetLanguageCode
                };
                var response = await _amazonTranslateClient.TranslateTextAsync(request).ConfigureAwait(false);
                return response.TranslatedText;
            }
            catch (Exception ex)
            {
                throw new AmazonException(ex.Message);
            }
        }
    }
}
