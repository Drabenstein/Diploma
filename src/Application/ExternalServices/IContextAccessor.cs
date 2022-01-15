namespace Application.ExternalServices;

public interface IContextAccessor
{
    public string AccessToken { get; }
}