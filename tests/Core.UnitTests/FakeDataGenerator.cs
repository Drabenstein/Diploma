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
            var faker = new Faker<User>()
                .RuleFor(x => x.Email, f => new Email(f.Internet.Email()))
                .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                .RuleFor(x => x.LastName, f => f.Name.LastName());
            return faker.Generate(1)[0];
        }

        public static Tutor GenerateTutor()
        {
            var faker = new Faker<Tutor>()
                    .RuleFor(x => x.Email, f => new Email(f.Internet.Email()))
                    .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                    .RuleFor(x => x.LastName, f => f.Name.LastName())
                    .RuleFor(x => x.AcademicDegree, f => f.PickRandom<AcademicDegree>())
                    .RuleFor(x => x.Department, f => f.Company.CompanyName())
                    .RuleFor(x => x.Pensum, f => f.Random.Int(10, 140))
                    .RuleFor(x => x.Position, f => f.PickRandom<TutorPosition>());
            return faker.Generate(1)[0];
        }

        public static Student GenerateStudent()
        {
            var faker = new Faker<Student>()
                .RuleFor(x => x.Email, f => new Email(f.Internet.Email()))
                .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                .RuleFor(x => x.LastName, f => f.Name.LastName())
                .RuleFor(x => x.IndexNumber, f => f.Random.Int(100_000, 999_999));
            return faker.Generate(1)[0];
        }

        public static FieldOfStudy GenerateFieldOfStudy()
        {
            var faker = new Faker<FieldOfStudy>()
                .RuleFor(x => x.Degree, f => f.PickRandom(1, 2))
                .RuleFor(x => x.HoursForThesis, f => f.Random.Int(10, 140))
                .RuleFor(x => x.LectureLanguage, f => f.PickRandom("PL", "EN"))
                .RuleFor(x => x.Name, () => "Informatyka stosowana")
                .RuleFor(x => x.StudyForm, f => f.PickRandom<StudyForm>());
            return faker.Generate(1)[0];
        }
    }
}
