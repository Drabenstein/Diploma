using Core.SeedWork;

namespace Core.Models.Users;

public record AreaOfInterest : EntityBase
{
    private readonly List<User> _users = new List<User>();
    
    public string Name { get; init; }

    public IReadOnlyList<User> Users => _users.AsReadOnly();
}