using Core.SeedWork;

namespace Core.Models.Users;

public record AreaOfInterest : EntityBase
{
    public string Name { get; init; }
}