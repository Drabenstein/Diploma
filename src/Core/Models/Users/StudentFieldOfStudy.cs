using Core.Models.Topics;
using Core.SeedWork;

namespace Core.Models.Users;

public record StudentFieldOfStudy
{
    public Student Student { get; set; }
    public FieldOfStudy FieldOfStudy { get; set; }
    public string Semester { get; set; }
    public string? Specialization { get; set; }
    public string? PlannedYearOfDefence { get; set; }
}