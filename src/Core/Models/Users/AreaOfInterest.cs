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
        if (_users.Contains(user))
        {
            throw new InvalidOperationException("Cannot add user to area of interest multiple times");
        }

        _users.Add(user);
    }
}