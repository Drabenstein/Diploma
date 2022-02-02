namespace Application.Queries.Dtos;

public class StudentsApprovedTopicDto
{
    public int TopicId { get; set; }
    public int ApplicationId { get; set; }
    public string Name { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string SupervisorAcademicDegree { get; set; } = null!;
    public string SupervisorFullName { get; set; } = null!;
    public string SupervisorDepartment { get; set; } = null!;
}