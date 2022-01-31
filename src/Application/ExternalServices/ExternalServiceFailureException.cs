namespace Application.ExternalServices;

public class ExternalServiceFailureException : Exception
{
    public ExternalServiceFailureException() : base()
    {
    }

    public ExternalServiceFailureException(string? message) : base(message)
    {
    }

    public ExternalServiceFailureException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
