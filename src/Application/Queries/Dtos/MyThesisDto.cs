namespace Application.Queries.Dtos;

public class MyThesisDto
{
    public long Id { get; set; }
    public string TopicName { get; set; }
    public string TopicEnglishName { get; set; }
    public string Status { get; set; }
    public string? Language { get; set; }
    public bool? HasConsentToChangeLanguage { get; set; }
    public string SupervisorFullName { get; set; }
    public string YearOfDefence { get; set; }
    public string FieldOfStudy { get; set; }
    
    public IEnumerable<ReviewForMyThesisDto> Reviews { get; set; }
}

