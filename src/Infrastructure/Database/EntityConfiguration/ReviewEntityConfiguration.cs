using Core.Models.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class ReviewEntityConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Grade)
            .HasConversion<string?>();

        builder.Property(x => x.IsPublished)
            .IsRequired();

        builder.Property(x => x.PublishTimestamp);
        
        builder.HasMany(x => x.ReviewModules)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.ReviewModules)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}