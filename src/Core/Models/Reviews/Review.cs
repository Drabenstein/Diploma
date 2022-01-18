using System.Diagnostics.CodeAnalysis;
using Core.Models.Reviews.ValueObjects;
using Core.Models.Users;
using Core.SeedWork;

namespace Core.Models.Reviews;

public record Review : EntityBase
{
    private readonly List<ReviewModule> _reviewModules = new List<ReviewModule>();
    
    // EF Core only
    [ExcludeFromCodeCoverage]
    private Review() { }

    public Review(Tutor reviewer)
    {
        this.Reviewer = reviewer;
    }
    
    public Tutor Reviewer { get; set; }
    public Grade? Grade { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishTimestamp { get; set; }
    public virtual IReadOnlyCollection<ReviewModule> ReviewModules => _reviewModules.AsReadOnly();
}