using Core.Models.Topics.ValueObjects;
using Core.Models.Users;
using Core.SeedWork;

namespace Core.Models.Topics;

public record FieldOfStudy : EntityBase
{
    private readonly List<StudentFieldOfStudy> _studentFieldsOfStudy = new List<StudentFieldOfStudy>();
    
    public string Name { get; set; }
    public string LectureLanguage { get; set; }
    public StudyForm StudyForm { get; set; }
    public int HoursForThesis { get; set; }
    public int Degree { get; set; }
    public virtual IReadOnlyCollection<StudentFieldOfStudy> StudentFieldsOfStudy => _studentFieldsOfStudy.AsReadOnly();
}