using Core.Models.Users;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class StudentFieldOfStudyEntityConfiguration : IEntityTypeConfiguration<StudentFieldOfStudy>
{
    public void Configure(EntityTypeBuilder<StudentFieldOfStudy> builder)
    {
        builder.ToTable(nameof(StudentFieldOfStudy));

        builder.HasKey(x => new { x.StudentId, x.FieldOfStudyId});

        builder.Property(x => x.Semester)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Specialization)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255);

        builder.Property(x => x.PlannedYearOfDefence)
            .HasColumnNameSnakeCased()
            .HasMaxLength(255);

        builder.HasOne(x => x.Student)
            .WithMany(s => s.StudentFieldOfStudies)
            .HasForeignKey(x => x.StudentId)
            .IsRequired();
        
        builder.HasOne(x => x.FieldOfStudy)
            .WithMany(x => x.StudentFieldsOfStudy)
            .HasForeignKey(x => x.FieldOfStudyId)
            .IsRequired();
    }
}