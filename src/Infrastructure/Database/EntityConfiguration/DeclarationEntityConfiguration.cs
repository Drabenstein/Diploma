using Core.Models.Theses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class DeclarationEntityConfiguration : IEntityTypeConfiguration<Declaration>
{
    public void Configure(EntityTypeBuilder<Declaration> builder)
    {
        builder.ToTable("Declarations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.ObjectiveOfWork)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.OperatingRange)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.Language)
            .HasMaxLength(255)
            .IsRequired();
    }
}