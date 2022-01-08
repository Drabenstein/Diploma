using System.Diagnostics.CodeAnalysis;
using Core.Models.Users.ValueObjects;

namespace Core.Models.Users;

public record Tutor : User
{
    // EF Core only
    [ExcludeFromCodeCoverage]
    private Tutor() { }
    
    public Tutor(string firstName, string lastName, Email email, IEnumerable<Role> roles, int pensum,
        TutorPosition position, string department, AcademicDegree academicDegree)
        : base(firstName, lastName, email, roles)
    {
        Pensum = pensum;
        Position = position;
        HasConsentToExtendPensum = false;
        Department = department;
        AcademicDegree = academicDegree;
        IsPositiveFacultyOpinion = false;
    }

    public int Pensum { get; set; }
    public TutorPosition Position { get; set; }
    public bool HasConsentToExtendPensum { get; set; }
    public string Department { get; set; }
    public AcademicDegree AcademicDegree { get; set; }
    public bool IsPositiveFacultyOpinion { get; set; }
}