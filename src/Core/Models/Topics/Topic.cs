using Core.Models.Theses;
using Core.Models.Users;
using Core.SeedWork;

namespace Core.Models.Topics;

public record Topic : EntityBase
{
    private readonly List<Thesis> _theses = new List<Thesis>();

    public User Proposer { get; set; }
    public Tutor Supervisor { get; set; }
    public string Name { get; set; }
    public string EnglishName { get; set; }
    public bool? IsAccepted { get; set; }
    public bool IsFree { get; set; }
    public int MaxRealizationNumber { get; set; }
    public string YearOfDefence { get; set; }
    public bool IsProposedByStudent { get; set; }
    public FieldOfStudy FieldOfStudy { get; set; }
    public virtual IReadOnlyCollection<Thesis> Theses => _theses.AsReadOnly();
}