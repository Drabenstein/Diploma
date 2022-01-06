using Core.Models.Users.ValueObjects;
using Core.SeedWork;

namespace Core.Models.Users;

public record User : EntityBase
{
    private readonly List<Role> _roles;
    private readonly List<AreaOfInterest> _areasOfInterest;
    
    public User(string firstName, string lastName, Email email, IEnumerable<Role> roles)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        _roles = new List<Role>(roles);
        _areasOfInterest = new List<AreaOfInterest>();
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Email Email { get; set; }
    public IReadOnlyList<Role> Roles => _roles.AsReadOnly();
    public IReadOnlyList<AreaOfInterest> AreasOfInterest => _areasOfInterest.AsReadOnly();
}