using Core.Models.Topics;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class FieldOfStudyEntityConfiguration : IEntityTypeConfiguration<FieldOfStudy>
{
    public void Configure(EntityTypeBuilder<FieldOfStudy> builder)
    {
        builder.ToTable(nameof(FieldOfStudy));

        builder.HasKey(x => x.Id);
        
        builder.HasIdColumnSnakeCased();

        builder.Property(x => x.Name)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.LectureLanguage)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.StudyForm)
            .HasColumnNameSnakeCased()
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.HoursForThesis)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.Property(x => x.Degree)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.HasMany(x => x.StudentFieldsOfStudy)
            .WithOne(x => x.FieldOfStudy)
            .HasForeignKey(x => x.FieldOfStudyId)
            .IsRequired();
        
        builder.Navigation(x => x.StudentFieldsOfStudy)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}