using Bogus;
using Core.Models.Topics;
using Core.Models.Topics.ValueObjects;
using Core.Models.Users;
using Core.Models.Users.ValueObjects;

namespace Core.UnitTests
{
    public static class FakeDataGenerator
    {
        public static User GenerateUser()
        {
            var f = new Faker();
            var email = new Email(f.Internet.Email());
            var firstName = f.Name.FirstName();
            var lastName = f.Name.LastName();
            return new User(firstName, lastName, email);
        }

        public static Tutor GenerateTutor()
        {
            var f = new Faker();
            var email = new Email(f.Internet.Email());
            var firstName = f.Name.FirstName();
            var lastName = f.Name.LastName();
            var academicDegree = f.PickRandom<AcademicDegree>();
            var department = f.Company.CompanyName();
            var pensum = f.Random.Int(10, 140);
            var position = f.PickRandom<TutorPosition>();
            return new Tutor(firstName, lastName, email, pensum, position, department, academicDegree);
        }

        public static Student GenerateStudent()
        {
            var f = new Faker();
            var email = new Email(f.Internet.Email());
            var firstName = f.Name.FirstName();
            var lastName = f.Name.LastName();
            var indexNumber = f.Random.Int(100_000, 999_999);
            return new Student(firstName, lastName, email, indexNumber);
        }

        public static FieldOfStudy GenerateFieldOfStudy()
        {
            var f = new Faker();
            var degree = f.PickRandom(1, 2);
            var hoursForThesis = f.Random.Int(10, 140);
            var lectureLanguage = f.PickRandom("PL", "EN");
            var name = "Informatyka stosowana";
            var studyForm = f.PickRandom<StudyForm>();
            return new FieldOfStudy
            {
                Degree = degree,
                HoursForThesis = hoursForThesis,
                LectureLanguage = lectureLanguage,
                Name = name,
                StudyForm = studyForm
            };
        }

        public static Application GenerateApplication(Topic topic, Student? submitter = null, bool isProposal = false)
        {
            var f = new Faker();
            return new Application
            {
                Topic = topic,
                IsTopicProposal = isProposal,
                Message = f.Random.Words(10),
                Submitter = submitter ?? GenerateStudent(),
                Timestamp = f.Date.Recent(5)
            };
        }
    }
}