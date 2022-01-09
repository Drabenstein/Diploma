using Core.Models.Reviews;
using Core.Models.Theses.ValueObjects;
using Core.Models.Topics;
using Core.Models.Users;
using Core.SeedWork;

namespace Core.Models.Theses;

public record Thesis : EntityBase
{
    private readonly List<Declaration> _declarations = new List<Declaration>();
    private readonly List<Review> _reviews = new List<Review>();

    public Topic Topic { get; set; }
    public ThesisStatus Status { get; set; }
    public byte[]? Content { get; set; }
    public ThesisFileFormat? FileFormat { get; set; }
    public ThesisLanguage? Language { get; set; }
    public bool? HasConsentToChangeLanguage { get; set; }
    public Student RealizerStudent { get; set; }
    public virtual IReadOnlyCollection<Declaration> Declarations => _declarations.AsReadOnly();
    public virtual IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
}