using Core.Models.Reviews;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class ReviewEntityConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(nameof(Review));

        builder.HasKey(x => x.Id);

        builder.HasIdColumnSnakeCased();
        
        builder.Property(x => x.Grade)
            .HasConversion<string?>()
            .HasColumnNameSnakeCased();

        builder.Property(x => x.IsPublished)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.Property(x => x.PublishTimestamp)
            .HasColumnNameSnakeCased();
        
        builder.HasMany(x => x.ReviewModules)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.ReviewModules)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}