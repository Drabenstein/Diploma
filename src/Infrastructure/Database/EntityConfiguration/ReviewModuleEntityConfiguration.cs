using Core.Models.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class ReviewModuleEntityConfiguration : IEntityTypeConfiguration<ReviewModule>
{
    public void Configure(EntityTypeBuilder<ReviewModule> builder)
    {
        builder.ToTable("ReviewModules");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Value)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired();
    }
}