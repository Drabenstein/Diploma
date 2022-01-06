using Core.Models.Users;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class AreaOfInterestEntityConfiguration : IEntityTypeConfiguration<AreaOfInterest>
{
    public void Configure(EntityTypeBuilder<AreaOfInterest> builder)
    {
        builder.ToTable(nameof(AreaOfInterest));
        
        builder.HasIdColumnSnakeCased();

        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();
    }
}