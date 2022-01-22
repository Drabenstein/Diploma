using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Application.Amazon;
using Core.Amazon;
using Microsoft.AspNetCore.StaticFiles;

namespace Infrastructure.AWS
{
    public class S3Service : IS3Service
    {
        private readonly AmazonS3Client _amazonS3Client;

        public S3Service(AmazonS3Client amazonS3Client)
        {
            _amazonS3Client = amazonS3Client;
        }

        public async Task UploadThesisAsync(string bucket, string key, byte[] content)
        {
            try
            {
                var transferUtility = new TransferUtility(_amazonS3Client);
                await transferUtility.UploadAsync(new MemoryStream(content), bucket, key).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AmazonException(ex.Message);
            }
        }

        public async Task<Stream> GetThesisAsync(string bucket, string key)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucket,
                Key = key
            };
            try
            {
                var response = await _amazonS3Client.GetObjectAsync(request).ConfigureAwait(false);
                new FileExtensionContentTypeProvider().TryGetContentType(response.Key, out var contentType);
                return response.ResponseStream;
            }
            catch (Exception ex)
            {
                throw new AmazonException(ex.Message);
            }
        }

        public async Task RemoveThesisAsync(string bucket, string key)
        {
            try
            {
                var transferUtility = new TransferUtility(_amazonS3Client);
                await _amazonS3Client.DeleteObjectAsync(bucket, key).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AmazonException(ex.Message);
            }
        }

    }
}
