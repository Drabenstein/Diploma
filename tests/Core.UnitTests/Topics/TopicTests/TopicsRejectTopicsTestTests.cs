using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.TopicTests;

public class TopicsRejectTopicsTestTests : TopicsTestBase
{
    [Fact]
    public void WhenTopicIsRejected_ShouldChangeAcceptanceFlag()
    {
        topic.RejectTopic();

        topic.IsAccepted.Should().BeFalse();
    }
    
    [Fact]
    public void WhenTopisIsAlreadyConsidered_ShouldThrow()
    {
        topic.RejectTopic();

        Action sut = () => topic.RejectTopic();

        sut.Should().Throw<InvalidOperationException>();
    }
}