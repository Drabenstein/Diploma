using Core.Models.Topics;
using Core.Models.Users;

namespace Core.UnitTests.Topics;

public abstract class TopicTestBase
{
    protected readonly Topic topic;
    protected readonly Application application;
    protected readonly Student student;

    public TopicTestBase()
    {
        topic = new Topic
        {
            Name = "Informatyka stosowana",
            EnglishName = "Applied computer science",
            IsFree = true,
            Proposer = FakeDataGenerator.GenerateUser(),
            IsProposedByStudent = false,
            MaxRealizationNumber = 2,
            FieldOfStudy = FakeDataGenerator.GenerateFieldOfStudy(),
            Supervisor = FakeDataGenerator.GenerateTutor(),
            YearOfDefence = "2021/2022"
        };
        student = FakeDataGenerator.GenerateStudent();
        application = FakeDataGenerator.GenerateApplication(topic, student);
    }
}