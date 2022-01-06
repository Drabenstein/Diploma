using Core.SeedWork;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Utilities;

public static class ColumnNamesExtensions
{
    /// <summary>
    /// Maps property to snake-cased column based on property name
    /// </summary>
    /// <param name="propertyBuilder"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static PropertyBuilder<T> HasColumnNameSnakeCased<T>(this PropertyBuilder<T> propertyBuilder)
    {
        return propertyBuilder.HasColumnName(propertyBuilder.Metadata.Name.Underscore());
    }
    
    /// <summary>
    /// Maps property to snake-cased column based on provided name
    /// </summary>
    /// <param name="propertyBuilder"></param>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static PropertyBuilder<T> HasColumnNameSnakeCased<T>(this PropertyBuilder<T> propertyBuilder, string name)
    {
        return propertyBuilder.HasColumnName(name.Underscore());
    }

    public static void HasIdColumnSnakeCased<T>(this EntityTypeBuilder<T> entityTypeBuilder)
        where T : EntityBase
    {
        entityTypeBuilder.Property(x => x.Id)
            .HasColumnNameSnakeCased($"{typeof(T).Name}{nameof(EntityBase.Id)}");
    }
}