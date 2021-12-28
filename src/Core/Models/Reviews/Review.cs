using Core.Models.Reviews.ValueObjects;
using Core.SeedWork;

namespace Core.Models.Reviews;

public record Review : EntityBase
{
    private readonly List<ReviewModule> _reviewModules = new List<ReviewModule>();
    
    public Grade? Grade { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishTimestamp { get; set; }
    public IReadOnlyList<ReviewModule> ReviewModules => _reviewModules.AsReadOnly();
}