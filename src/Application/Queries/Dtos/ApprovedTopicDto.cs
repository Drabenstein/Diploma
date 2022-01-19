namespace Application.Queries.Dtos;
public class ApprovedTopicDto
{
    public long TopicId { get; set; }
    public long ApplicationId { get; set; }
    public string Name { get; set; }
    public string EnglishName { get; set; }
    public string Supervisor { get; set; }
    public string ApplicationStatus { get; set; }
}