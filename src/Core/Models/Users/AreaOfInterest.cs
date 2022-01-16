using Core.SeedWork;

namespace Core.Models.Users;

public record AreaOfInterest : EntityBase
{
    private readonly List<User> _users = new List<User>();
    
    public string Name { get; init; }

    public virtual IReadOnlyCollection<User> Users => _users.AsReadOnly();

    public void RemoveUser(User user)
    {
        _users.Remove(user);
    }

    public void AddUser(User user)
    {
        _users.Add(user);
    }

}