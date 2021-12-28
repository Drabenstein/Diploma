using Core.Models.Topics.ValueObjects;
using Core.Models.Users;
using Core.SeedWork;

namespace Core.Models.Topics;

public record Application : EntityBase
{
    public User Submitter { get; set; }
    public Topic? Topic { get; set; }
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
    public ApplicationStatus Status { get; set; }
    public bool IsTopicProposal { get; set; }
}