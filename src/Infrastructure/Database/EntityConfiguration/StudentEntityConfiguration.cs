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

        builder.HasIndex(x => x.IndexNumber)
            .IsUnique();

        builder.HasMany(x => x.StudentFieldOfStudies)
            .WithOne();

        builder.Navigation(x => x.StudentFieldOfStudies)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}