using Core.Models.Reviews.ValueObjects;
using Core.SeedWork;

namespace Core.Models.Reviews;

public record ReviewModule : EntityBase
{
    public string Name { get; set; }
    public string Value { get; set; }
    public ReviewModuleType Type { get; set; }
    public void SetValue(string value)
    {
        if (Type == ReviewModuleType.Number)
        {
            var parsed = double.TryParse(value, out var _);
            if (parsed) Value = value;
            else throw new InvalidOperationException($"Cannot assign such grade: {value}");
        }
        Value = value;
    }
}