namespace Application.Queries.Dtos;

public class ReviewerWithInterestsDto
{
    public string Title { get; set; }
    public string Employee { get; set; }
    public string Department { get; set; }
    public string Position { get; set; }
    public int ReviewsNumber { get; set; }
    public long[] AreasOfInterestIds { get; set;}
}
