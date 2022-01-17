namespace Application.Queries.Dtos;
public class ReviewersThesisDto
{
    public long ThesisId { get; set; }
    public string Title { get; set; }
    public string EnglishTitle { get; set; }
    public string Supervisor { get; set; }
    public string Realizer { get; set; }
    public string Status { get; set; }
    public string Department { get; set; }
}

