using Core.Models.Users.ValueObjects;

namespace Core.Models.Users;

public record Student : User
{
    public Student() { }
    private readonly List<StudentFieldOfStudy> _studentFieldOfStudies;

    public Student(string firstName, string lastName, IEnumerable<Role> roles, int indexNumber) : base(
        firstName, lastName, Email.CreateStudentEmail(indexNumber), roles)
    {
        IndexNumber = indexNumber;
        _studentFieldOfStudies = new List<StudentFieldOfStudy>();
    }

    public int IndexNumber { get; set; }
    public IReadOnlyList<StudentFieldOfStudy> StudentFieldOfStudies => _studentFieldOfStudies.AsReadOnly();
}