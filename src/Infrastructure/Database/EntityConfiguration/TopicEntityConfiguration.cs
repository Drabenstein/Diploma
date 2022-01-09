using Core.Models.Topics;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class TopicEntityConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable(nameof(Topic));
        
        builder.HasIdColumnSnakeCased();

        builder.Property(x => x.Name)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.EnglishName)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.IsAccepted)
            .HasColumnNameSnakeCased();

        builder.Property(x => x.IsFree)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.Property(x => x.MaxRealizationNumber)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.Property(x => x.YearOfDefence)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.Property(x => x.IsProposedByStudent)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.HasOne(x => x.FieldOfStudy)
            .WithMany()
            .IsRequired();

        builder.HasMany(x => x.Theses)
            .WithOne(x => x.Topic);

        builder.Navigation(x => x.Theses)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(x => x.Proposer)
            .WithMany()
            .IsRequired();

        builder.HasOne(x => x.Supervisor)
            .WithMany()
            .IsRequired();

    }
}