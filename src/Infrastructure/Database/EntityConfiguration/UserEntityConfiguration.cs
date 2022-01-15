using Core.Models.Users;
using Core.Models.Users.ValueObjects;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));
        
        builder.HasIdColumnSnakeCased();

        builder.Property(x => x.FirstName)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnNameSnakeCased()
            .HasConversion(x => x.Address, s => new Email(s))
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.HasMany(x => x.AreasOfInterest)
            .WithMany(x => x.Users);

        builder.Navigation(x => x.AreasOfInterest)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}