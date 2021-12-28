using Core.SeedWork;

namespace Core.Models.Reviews;

public record ReviewModule : EntityBase
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
}