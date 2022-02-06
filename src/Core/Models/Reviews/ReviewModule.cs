using System.Globalization;
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
        if (Type == ReviewModuleType.Number &&
            !double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var _))
        {
            throw new InvalidOperationException($"Cannot assign such grade: {value}");
        }

        Value = value;
    }
}