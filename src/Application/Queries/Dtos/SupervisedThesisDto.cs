namespace Application.Queries.Dtos;

public class SupervisedThesisDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string EnglishName { get; set; }
    public string SupervisorAcademicDegree { get; set; }
    public string SupervisorFullName { get; set; }
    public string? ReviewerAcademicDegree { get; set; }
    public string? ReviewerFullName { get; set; }
    public string StudentFullName { get; set; }
}