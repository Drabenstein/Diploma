using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.TopicTests;

public class TopicAcceptTopicTestTests : TopicTestBase
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