namespace Core.Amazon
{
    public class AmazonException : Exception
    {
        public AmazonException()
        {
        }

        public AmazonException(string? message) : base(message)
        {
        }

        public AmazonException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
