namespace Application.Queries.Dtos;

public class StudentsApprovedTopicDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string SupervisorAcademicDegree { get; set; } = null!;
    public string SupervisorFullName { get; set; } = null!;
    public string SupervisorDepartment { get; set; } = null!;
}