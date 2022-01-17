namespace Application.Queries.Dtos;

public class TopicForConsiderationDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public int MaxRealizationNumber { get; set; }
    public string SupervisorAcademicDegree { get; set; } = null!;
    public string SupervisorFullName { get; set; } = null!;
    public string SupervisorDepartment { get; set; } = null!;
}