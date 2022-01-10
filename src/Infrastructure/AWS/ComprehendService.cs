using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using Application.Amazon;
using Core.Amazon;

namespace Infrastructure.AWS
{
    public class ComprehendService : IComprehendService
    {
        private readonly AmazonComprehendClient _amazonComprehendClient;

        public ComprehendService(AmazonComprehendClient amazonComprehendClient)
        {
            _amazonComprehendClient = amazonComprehendClient;
        }

        public async Task<string?> GetDominantLanguageAsync(string text)
        {
            try
            {
                var request = new DetectDominantLanguageRequest { Text = text };
                var response = await _amazonComprehendClient.DetectDominantLanguageAsync(request).ConfigureAwait(false);
                return response
                    .Languages
                    .OrderByDescending(x => x.Score)
                    .Select(x => x.LanguageCode)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new AmazonException(ex.Message);
            }

        }

        public async Task<string> GetSentimentAsync(string text, string languageCode)
        {
            try
            {
                var request = new DetectSentimentRequest { Text = text, LanguageCode = languageCode };
                var response = await _amazonComprehendClient.DetectSentimentAsync(request).ConfigureAwait(false);
                return response.Sentiment.Value;
            }
            catch (Exception ex)
            {
                throw new AmazonException(ex.Message);
            }
        }

        public async Task<int> FilterWordInTextAsync(string text, string languageCode, string word)
        {
            try
            {
                var request = new DetectSyntaxRequest { Text = text, LanguageCode = languageCode };
                var response = await _amazonComprehendClient.DetectSyntaxAsync(request);
                return response.SyntaxTokens.Where(x => x.Text.Equals(word)).Count();
            }
            catch (Exception ex)
            {
                throw new AmazonException(ex.Message);
            }
        }
    }
}
