namespace Application.Queries.Dtos;

public class ApplicationDetailsDto
{
    public string StudentFullName { get; set; }
    public int StudentIndex { get; set; }
    public string FieldOfStudyName { get; set; }
    public int FieldOfStudyDegree { get; set; }
    public string FieldOfStudyStudyForm { get; set; }
    public string YearOfDefence { get; set; }
    public int TopicMaxRealizationNumber { get; set; }
    public int TopicCurrentRealizations { get; set; }
    public string TopicName { get; set; }
    public string TopicEnglishName { get; set; }
    public string Message { get; set; }
}