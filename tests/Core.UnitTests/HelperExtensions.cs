using System.Reflection;
using Core.SeedWork;

namespace Core.UnitTests;

public static class HelperExtensions
{
    public static void SetId<TEntity>(this TEntity entity, long id) where TEntity : EntityBase
    {
        PropertyInfo? property = typeof(TEntity).GetTypeInfo().GetProperty(nameof(EntityBase.Id), BindingFlags.Instance | BindingFlags.Public);
        property!.SetValue(entity, id, null);
    }
    
    public static void SetPropertyValue<TEntity, TValue>(this TEntity entity, string propertyName, TValue propertyValue) where TEntity : EntityBase
    {
        PropertyInfo? property = typeof(TEntity).GetTypeInfo().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
        property!.SetValue(entity, propertyValue, null);
    }
}