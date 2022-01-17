namespace Application.Queries.Dtos;

public class ReviewForMyThesisDto
{
    public long Id { get; set; }
    public string? Grade { get; set; }
    public DateTime? Timestamp { get; set; }
    public string Reviewer { get; set; }
}

