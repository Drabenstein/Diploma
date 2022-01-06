using Core.Models.Users;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role));
        
        builder.HasIdColumnSnakeCased();

        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();
    }
}