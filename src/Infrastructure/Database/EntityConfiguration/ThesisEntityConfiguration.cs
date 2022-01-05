using Core.Models.Theses;
using Core.Models.Theses.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class ThesisEntityConfiguration : IEntityTypeConfiguration<Thesis>
{
    public void Configure(EntityTypeBuilder<Thesis> builder)
    {
        builder.ToTable("Theses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Content);

        builder.Property(x => x.FileFormat)
            .HasColumnName("file_format")
            .HasColumnType("varchar(255)")
            .HasConversion<string?>(x => x != null ? x.Name : null, _ => ThesisFileFormat.Pdf);

        builder.Property(x => x.Language)
            .HasColumnName("file_format")
            .HasColumnType("varchar(255)")
            .HasConversion<string?>(x => x != null ? x.Language : null,
                s => s != null
                    ? (s.Equals("en", StringComparison.OrdinalIgnoreCase)
                        ? ThesisLanguage.English
                        : ThesisLanguage.Polish)
                    : null);

        builder.Property(x => x.HasConsentToChangeLanguage)
            .HasColumnType("bool");

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
            .IsRequired();
    }
}