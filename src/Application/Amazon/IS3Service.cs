namespace Application.Amazon
{
    public interface IS3Service
    {
        Task<Stream> GetThesisAsync(string bucket, string key);
        Task RemoveThesisAsync(string bucket, string key);
        Task UploadThesisAsync(string bucket, string key, byte[] content);
    }
}
