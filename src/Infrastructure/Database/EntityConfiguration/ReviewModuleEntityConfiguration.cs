using Core.Models.Reviews;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class ReviewModuleEntityConfiguration : IEntityTypeConfiguration<ReviewModule>
{
    public void Configure(EntityTypeBuilder<ReviewModule> builder)
    {
        builder.ToTable(nameof(ReviewModule));

        builder.HasKey(x => x.Id);

        builder.HasIdColumnSnakeCased();
        
        builder.Property(x => x.Name)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.Property(x => x.Value)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.Property(x => x.Type)
            .HasColumnNameSnakeCased()
            .IsRequired();
    }
}