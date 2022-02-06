using Core.Models.Topics.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.TopicTests;

public class TopicsAcceptApplicationTests : TopicsTestBase
{
    [Fact]
    public void WhenApplicationExists_ShouldAcceptIt()
    {
        topic.SubmitApplication(application);
            
        topic.AcceptApplication(application.Id);

        topic.Applications.First().Status.Should().Be(ApplicationStatus.Approved);
    }

    [Fact]
    public void WhenApplicationDoesNotExist_ShouldThrow()
    {
        Action sut = () => topic.AcceptApplication(application.Id);

        sut.Should().Throw<Exception>();
    }

    [Fact]
    public void WhenApplicationIsNotInInitialSentState_ShouldThrow()
    {
        topic.SubmitApplication(application);
        topic.CancelApplication(application.Id);

        Action sut = () => topic.AcceptApplication(application.Id);

        sut.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void WhenMaxApplicationsAreAcceptedOrConfirmed_ShouldThrow()
    {
        for (int i = 0; i < topic.MaxRealizationNumber; i++)
        {
            var additionalApplication = FakeDataGenerator.GenerateApplication(topic);
            topic.SubmitApplication(additionalApplication);
            additionalApplication.SetId(i + 1);
            topic.AcceptApplication(additionalApplication.Id);
        }
        topic.SubmitApplication(application);

        Action sut = () => topic.AcceptApplication(application.Id);

        sut.Should().Throw<InvalidOperationException>();
    }
}