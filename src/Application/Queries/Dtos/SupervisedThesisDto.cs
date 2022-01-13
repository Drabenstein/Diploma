namespace Application.Queries.Dtos;

public class SupervisedThesisDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public string SupervisorAcademicDegree { get; set; } = null!;
    public string SupervisorFullName { get; set; } = null!;
    public string? ReviewerAcademicDegree { get; set; }
    public string? ReviewerFullName { get; set; }
    public string StudentFullName { get; set; } = null!;
}