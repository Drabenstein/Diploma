namespace Infrastructure.Auth0;

public class Auth0ApiConfig
{
    public const string ConfigKey = "Auth0ApiConfig";

    public string Auth0Domain { get; set; } = string.Empty;
    public string GrantType { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
