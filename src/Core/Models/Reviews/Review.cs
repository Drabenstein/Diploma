using Core.Models.Reviews.ValueObjects;
using Core.Models.Users;
using Core.SeedWork;
using System.Diagnostics.CodeAnalysis;

namespace Core.Models.Reviews;

public record Review : EntityBase
{
    private readonly List<ReviewModule> _reviewModules = new List<ReviewModule>();

    // EF Core only
    [ExcludeFromCodeCoverage]
    private Review() { }

    public Review(Tutor reviewer)
    {
        Reviewer = reviewer;
        AddDefaultReviewModules();
    }

    public Tutor Reviewer { get; set; }
    public Grade? Grade { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishTimestamp { get; set; }
    public virtual IReadOnlyCollection<ReviewModule> ReviewModules => _reviewModules.AsReadOnly();

    private void AddDefaultReviewModules()
    {
        foreach (var module in DefaultReviewModules.GetModuleTemplates())
        {
            _reviewModules.Add(new ReviewModule
            {
                Name = module.Name,
                Type = module.Type,
                Value = module.Value
            });
        }
    }

    public void SubmitGrade(string grade)
    {
        if (Grade is not null) throw new InvalidOperationException("This review was already graded");

        Grade = grade switch
        {
            "2" => ValueObjects.Grade.Two,
            "3" => ValueObjects.Grade.Three,
            "3.5" => ValueObjects.Grade.ThreeAndHalf,
            "4" => ValueObjects.Grade.Four,
            "4.5" => ValueObjects.Grade.FourAndHalf,
            "5" => ValueObjects.Grade.Five,
            "5.5" => ValueObjects.Grade.FiveAndHalf,
            _ => throw new InvalidOperationException($"Only grades from range [2, 3, 3.5, 4, 4.5, 5, 5.5] can be assigned")
        };

        PublishTimestamp = DateTime.UtcNow;

        IsPublished = true;
    }
}