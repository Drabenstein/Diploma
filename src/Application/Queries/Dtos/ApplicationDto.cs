namespace Application.Queries.Dtos;

public class ApplicationDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public string StudentFullName { get; set; } = null!;
    public bool IsTopicProposal { get; set; }
}