namespace Application.Queries.Dtos;

public class StudentsReviewDataDto
{
    public long ReviewId { get; set; }
    public string Reviewer { get; set; }
    public string TopicName { get; set; }
    public string EnglishTopicName { get; set; }
    public IEnumerable<ReviewModuleWithValueDto> Modules { get; set; }
}

