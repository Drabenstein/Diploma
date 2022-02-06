using Core.Models.Topics.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.TopicTests;

public class TopicsRejectApplicationTests : TopicsTestBase
{
    [Fact]
    public void WhenApplicationExists_ShouldRejectIt()
    {
        topic.SubmitApplication(application);
            
        topic.RejectApplication(application.Id);

        topic.Applications.First().Status.Should().Be(ApplicationStatus.Rejected);
    }

    [Fact]
    public void WhenApplicationDoesNotExist_ShouldThrow()
    {
        Action sut = () => topic.RejectApplication(application.Id);

        sut.Should().Throw<Exception>();
    }

    [Fact]
    public void WhenApplicationIsNotInInitialSentState_ShouldThrow()
    {
        topic.SubmitApplication(application);
        topic.CancelApplication(application.Id);

        Action sut = () => topic.RejectApplication(application.Id);

        sut.Should().Throw<InvalidOperationException>();
    }
}