using Core.Models.Topics;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class ApplicationEntityConfiguration : IEntityTypeConfiguration<Core.Models.Topics.Application>
{
    public void Configure(EntityTypeBuilder<Core.Models.Topics.Application> builder)
    {
        builder.ToTable(nameof(Application));
        
        builder.HasIdColumnSnakeCased();

        builder.Property(x => x.Timestamp)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.Property(x => x.Message)
            .HasColumnNameSnakeCased()
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnNameSnakeCased()
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.IsTopicProposal)
            .HasColumnNameSnakeCased()
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne(x => x.Submitter)
            .WithMany()
            .IsRequired();

        builder.HasOne(x => x.Topic)
            .WithMany();
    }
}