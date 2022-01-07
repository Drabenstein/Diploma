using Core.Models.Users;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class TutorEntityConfiguration : IEntityTypeConfiguration<Tutor>
{
    public void Configure(EntityTypeBuilder<Tutor> builder)
    {
        builder.Property(x => x.Pensum)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.Property(x => x.Position)
            .HasColumnNameSnakeCased()
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.HasConsentToExtendPensum)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.Property(x => x.Department)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.AcademicDegree)
            .HasColumnNameSnakeCased()
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.IsPositiveFacultyOpinion)
            .HasColumnNameSnakeCased()
            .IsRequired();
    }
}