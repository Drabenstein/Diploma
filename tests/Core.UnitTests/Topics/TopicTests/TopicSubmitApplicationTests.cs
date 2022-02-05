using Core.Models.Topics;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.TopicTests;

public class TopicSubmitApplicationTests : TopicTestBase
{
    [Fact]
    public void WhenApplicationIsSubmitted_ShouldBeSaved()
    {
        topic.SubmitApplication(application);

        topic.Applications.Should().HaveCount(1);
        topic.Applications.First().Should().Be(application);
    }
    
    [Fact]
    public void WhenApplicationFromSubmitterAlreadyExists_ShouldThrow()
    {
        topic.SubmitApplication(application);
        var duplicateApplication = new Application()
        {
            Topic = topic,
            IsTopicProposal = false,
            Message = "Test message",
            Submitter = student,
            Timestamp = new DateTime(2022, 01, 10, 10, 12, 11)
        };

        Action sut = () => topic.SubmitApplication(duplicateApplication);

        sut.Should().Throw<InvalidOperationException>();
    }
}