using System.Runtime.CompilerServices;
using Core.Models.Theses;
using Core.Models.Topics.ValueObjects;
using Core.Models.Users;
using Core.SeedWork;

namespace Core.Models.Topics;

public record Topic : EntityBase
{
    private readonly List<Thesis> _theses = new List<Thesis>();
    private readonly List<Application> _applications = new List<Application>();

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
    public virtual IReadOnlyCollection<Application> Applications => _applications.AsReadOnly();

    public void AcceptApplication(long applicationId)
    {
        var application = _applications.Single(x => x.Id == applicationId);
        if (_applications.Count(x => x.Status is ApplicationStatus.Approved or ApplicationStatus.Confirmed) >= MaxRealizationNumber)
        {
            throw new InvalidOperationException("Topic already has maximum number of approved/confirmed students");
        }
        
        application.AcceptApplication();
    }
    
    public void RejectApplication(long applicationId)
    {
        var application = _applications.Single(x => x.Id == applicationId);
        application.RejectApplication();
    }
    
    public void ConfirmApplication(long applicationId)
    {
        var application = _applications.Single(x => x.Id == applicationId);
        application.ConfirmApplication();
        IsFree = _applications.Count(x => x.Status == ApplicationStatus.Confirmed) == MaxRealizationNumber;
        var thesis = new Thesis(this, application.Submitter);
        _theses.Add(thesis);
    }
    
    public void CancelApplication(long applicationId)
    {
        var application = _applications.Single(x => x.Id == applicationId);
        application.CancelApplication();
    }
}