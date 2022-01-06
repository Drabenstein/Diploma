using Core.Models.Theses;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class DeclarationEntityConfiguration : IEntityTypeConfiguration<Declaration>
{
    public void Configure(EntityTypeBuilder<Declaration> builder)
    {
        builder.ToTable(nameof(Declaration));

        builder.HasKey(x => x.Id);

        builder.HasIdColumnSnakeCased();
        
        builder.Property(x => x.Date)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.Property(x => x.ObjectiveOfWork)
            .HasColumnNameSnakeCased()
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.OperatingRange)
            .HasColumnNameSnakeCased()
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.Language)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255)
            .IsRequired();
    }
}