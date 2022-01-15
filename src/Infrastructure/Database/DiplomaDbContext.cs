using Core.Models.Reviews;
using Core.Models.Theses;
using Core.Models.Topics;
using Core.Models.Users;
using Core.SeedWork;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class DiplomaDbContext : DbContext
{
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ReviewModule> ReviewModules { get; set; }
    
    public DbSet<Thesis> Theses { get; set; }
    public DbSet<Declaration> Declarations { get; set; }

    public DbSet<Topic> Topics { get; set; }
    public DbSet<Core.Models.Topics.Application> Applications { get; set; }
    public DbSet<FieldOfStudy> FieldsOfStudies { get; set; }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Tutor> Tutors { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<AreaOfInterest> AreasOfInterests { get; set; }

#pragma warning disable CS8618
    public DiplomaDbContext() { }
    public DiplomaDbContext(DbContextOptions options) : base(options) { }
#pragma warning restore CS8618
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiplomaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
        
        foreach(var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName().Underscore());
            
            foreach(var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnBaseName().Underscore());
            }

            foreach(var key in entity.GetKeys())
            {
                key.SetName(key.GetName().Underscore());
            }

            foreach(var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(key.GetConstraintName().Underscore());
            }

            foreach(var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName().Underscore());
            }
        }
    }

    public Task CommitAsync()
    {
        return this.SaveChangesAsync();
    }
}