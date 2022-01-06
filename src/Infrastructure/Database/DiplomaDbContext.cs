using Core.Models.Reviews;
using Core.Models.Theses;
using Core.Models.Topics;
using Core.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class DiplomaDbContext : DbContext
{
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ReviewModule> ReviewModules { get; set; }
    
    public DbSet<Thesis> Theses { get; set; }
    public DbSet<Declaration> Declarations { get; set; }

    public DbSet<Topic> Topics { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<FieldOfStudy> FieldsOfStudies { get; set; }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Tutor> Tutors { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<AreaOfInterest> AreasOfInterests { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<StudentFieldOfStudy> StudentFieldsOfStudies { get; set; }

    public DiplomaDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiplomaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}