using Core.Models.Topics.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.TopicTests;

public class TopicConfirmApplicationTests : TopicTestBase
{
    [Fact]
    public void WhenApplicationDoesNotExist_ShouldThrow()
    {
        Action sut = () => topic.ConfirmApplication(application.Id);

        sut.Should().Throw<Exception>();
    }

    [Fact]
    public void WhenApplicationExistsAndHasAcceptedState_ShouldConfirmIt()
    {
        topic.SubmitApplication(application);
        topic.AcceptApplication(application.Id);

        topic.ConfirmApplication(application.Id);

        topic.Applications.First().Status.Should().Be(ApplicationStatus.Confirmed);
        topic.IsFree.Should().BeTrue();
    }

    [Fact]
    public void WhenApplicationExistsButIsNotAccepted_ShouldThrow()
    {
        topic.SubmitApplication(application);

        Action sut = () => topic.ConfirmApplication(application.Id);

        sut.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void WhenApplicationIsConfirmedAndMaxRealizationsAreMet_ShouldMarkTopicAsNotFree()
    {
        topic.SubmitApplication(application);
        var anotherApplication = FakeDataGenerator.GenerateApplication(topic);
        anotherApplication.SetId(1);
        topic.SubmitApplication(anotherApplication);
        topic.AcceptApplication(anotherApplication.Id);
        topic.ConfirmApplication(anotherApplication.Id);
        topic.AcceptApplication(application.Id);
        
        topic.ConfirmApplication(application.Id);

        topic.IsFree.Should().BeFalse();
    }

    [Fact]
    public void WhenApplicationIsConfirmed_ShouldCreateThesisForStudent()
    {
        topic.SubmitApplication(application);
        topic.AcceptApplication(application.Id);
        
        topic.ConfirmApplication(application.Id);

        topic.Theses.Should().HaveCount(1);
        topic.Theses.First().Topic.Should().Be(topic);
        topic.Theses.First().RealizerStudent.Should().Be(student);
    }
}