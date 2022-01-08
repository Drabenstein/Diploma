using Core.Models.Topics;
using Core.SeedWork;

namespace Core.Models.Users;

public record StudentFieldOfStudy
{
    public long StudentId { get; set; }
    public Student Student { get; set; }
    public long FieldOfStudyId { get; set; }
    public FieldOfStudy FieldOfStudy { get; set; }
    public string Semester { get; set; }
    public string? Specialization { get; set; }
    public string? PlannedYearOfDefence { get; set; }
}