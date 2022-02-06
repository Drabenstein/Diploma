using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.TopicTests;

public class TopicsAcceptTopicsTestTests : TopicsTestBase
{
    [Fact]
    public void WhenTopicIsAccepted_ShouldChangeAcceptanceFlag()
    {
        topic.AcceptTopic();

        topic.IsAccepted.Should().BeTrue();
    }

    [Fact]
    public void WhenTopisIsAlreadyConsidered_ShouldThrow()
    {
        topic.AcceptTopic();

        Action sut = () => topic.AcceptTopic();

        sut.Should().Throw<InvalidOperationException>();
    }
}