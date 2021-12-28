using Core.Models.Topics.ValueObjects;
using Core.SeedWork;

namespace Core.Models.Topics;

public record FieldOfStudy : EntityBase
{
    public string Name { get; set; }
    public string LectureLanguage { get; set; }
    public StudyForm StudyForm { get; set; }
    public int HoursForThesis { get; set; }
    public int Degree { get; set; }
}