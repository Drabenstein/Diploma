namespace Application.Queries.Dtos;

public class ThesisForReviewerAssignmentDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public string SupervisorAcademicDegree { get; set; } = null!;
    public string SupervisorFullName { get; set; } = null!;
    public string SupervisorDepartment { get; set; } = null!;
    public string? ReviewerAcademicDegree { get; set; }
    public string? ReviewerFullName { get; set; }
    public long? ReviewerId { get; set; }
}