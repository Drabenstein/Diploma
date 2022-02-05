using System.Reflection;
using Core.SeedWork;

namespace Core.UnitTests;

public static class HelperExtensions
{
    public static void SetId<T>(this T entity, long id) where T : EntityBase
    {
        PropertyInfo? property = typeof(T).GetTypeInfo().GetProperty(nameof(EntityBase.Id), BindingFlags.Instance | BindingFlags.Public);
        property!.SetValue(entity, id, null);
    }
}