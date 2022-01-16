namespace Application.Common;

public class ValidationException : Exception
{
    public ValidationException(string? message) : base(message)
    {
    }

    public ValidationException() : base()
    {
    }

    public ValidationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}