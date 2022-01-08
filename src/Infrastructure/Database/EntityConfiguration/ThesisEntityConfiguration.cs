using Core.Models.Theses;
using Core.Models.Theses.ValueObjects;
using Core.Models.Users;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class ThesisEntityConfiguration : IEntityTypeConfiguration<Thesis>
{
    public void Configure(EntityTypeBuilder<Thesis> builder)
    {
        builder.ToTable(nameof(Thesis));

        builder.HasKey(x => x.Id);

        builder.HasIdColumnSnakeCased();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.Property(x => x.Content)
            .HasColumnNameSnakeCased();

        builder.Property(x => x.FileFormat)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255)
            .HasConversion<string?>(x => x != null ? x.Name : null, _ => ThesisFileFormat.Pdf);

        builder.Property(x => x.Language)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255)
            .HasConversion<string?>(x => x != null ? x.Language : null,
                s => s != null
                    ? (s.Equals("en", StringComparison.OrdinalIgnoreCase)
                        ? ThesisLanguage.English
                        : ThesisLanguage.Polish)
                    : null);

        builder.Property(x => x.HasConsentToChangeLanguage)
            .HasColumnNameSnakeCased();

        builder.HasOne(x => x.Topic)
            .WithMany(t => t.Theses);

        builder.HasMany(x => x.Declarations)
            .WithOne();

        builder.Navigation(x => x.Declarations)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(x => x.Reviews)
            .WithOne();

        builder.Navigation(x => x.Reviews)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(x => x.Student)
            .WithOne()
            .HasForeignKey<Student>(x => x.Id)
            .IsRequired();
    }
}