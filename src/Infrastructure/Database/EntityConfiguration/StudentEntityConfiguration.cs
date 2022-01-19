using Core.Models.Theses;
using Core.Models.Users;
using Infrastructure.Database.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.Property(x => x.IndexNumber)
            .HasColumnNameSnakeCased()
            .IsRequired();

        builder.HasIndex(x => x.IndexNumber);

        builder.HasMany(x => x.StudentFieldOfStudies)
            .WithOne(x => x.Student)
            .HasForeignKey(x => x.StudentId)
            .IsRequired();

        builder.Navigation(x => x.StudentFieldOfStudies)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(x => x.Theses)
            .WithOne(x => x.RealizerStudent);

        builder.Navigation(x => x.Theses)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(x => x.Applications)
            .WithOne(x => x.Submitter);

        builder.Navigation(x => x.Applications)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}