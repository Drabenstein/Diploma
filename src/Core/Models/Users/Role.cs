using Core.SeedWork;

namespace Core.Models.Users;

public record Role : EntityBase
{
    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}