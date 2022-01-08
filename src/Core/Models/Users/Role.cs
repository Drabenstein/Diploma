using Core.SeedWork;

namespace Core.Models.Users;

public record Role : EntityBase
{
    private readonly List<User> _users = new List<User>();
    
    public string Name { get; set; }

    public virtual IReadOnlyCollection<User> Users => _users.AsReadOnly();

    public override string ToString()
    {
        return Name;
    }
}