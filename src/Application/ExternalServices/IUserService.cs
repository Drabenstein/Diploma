namespace Application.ExternalServices;

public interface IUserService
{
    Task ChangePasswordAsync(string userExternalId, string password);
}
