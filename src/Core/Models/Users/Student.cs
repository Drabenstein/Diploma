using System.Diagnostics.CodeAnalysis;
using Core.Models.Users.ValueObjects;

namespace Core.Models.Users;

public record Student : User
{
    // EF Core only
    [ExcludeFromCodeCoverage]
    private Student() { }
    
    private readonly List<StudentFieldOfStudy> _studentFieldOfStudies;

    public Student(string firstName, string lastName, IEnumerable<Role> roles, int indexNumber) : base(
        firstName, lastName, Email.CreateStudentEmail(indexNumber), roles)
    {
        IndexNumber = indexNumber;
        _studentFieldOfStudies = new List<StudentFieldOfStudy>();
    }

    public int IndexNumber { get; set; }
    public virtual IReadOnlyCollection<StudentFieldOfStudy> StudentFieldOfStudies => _studentFieldOfStudies.AsReadOnly();
}