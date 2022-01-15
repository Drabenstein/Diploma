using Core.Models.Users;

namespace Application.ExternalServices;

public interface IUserDataFetcher
{
    Task<UserDataDto> FetchUserDataAsync(string email);
}