using Core.Models.Topics.ValueObjects;
using Core.Models.Users;
using Core.SeedWork;

namespace Core.Models.Topics;

public record Application : EntityBase
{
    public Student Submitter { get; set; }
    public Topic Topic { get; set; }
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
    public ApplicationStatus Status { get; private set; } = ApplicationStatus.Sent;
    public bool IsTopicProposal { get; set; }

    public void AcceptApplication()
    {
        if (Status == ApplicationStatus.Sent)
        {
            Status = ApplicationStatus.Approved;
        }
        else
        {
            throw new InvalidOperationException("Application can be only accepted when it has status Sent");
        }
    }
    
    public void RejectApplication()
    {
        if (Status == ApplicationStatus.Sent)
        {
            Status = ApplicationStatus.Rejected;
        }
        else
        {
            throw new InvalidOperationException("Application can be only rejected when it has status Sent");
        }
    }
    
    public void ConfirmApplication()
    {
        if (Status == ApplicationStatus.Approved)
        {
            Status = ApplicationStatus.Confirmed;
        }
        else
        {
            throw new InvalidOperationException("Application can be only confirmed when it has status Approved");
        }
    }
    
    public void CancelApplication()
    {
        if (Status == ApplicationStatus.Confirmed)
        {
            throw new InvalidOperationException("Application cannot be cancelled if it is already confirmed");
        }
        
        if (Status != ApplicationStatus.Rejected)
        {
            Status = ApplicationStatus.Cancelled;
        }
    }
}