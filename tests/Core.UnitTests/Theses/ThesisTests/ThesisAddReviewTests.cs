using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Theses.ThesisTests;

public class ThesisAddReviewTests
{
    [Fact]
    public void WhenDuplicateReviewAdded_ShouldThrow()
    {
        var thesis = FakeDataGenerator.GenerateThesis();
        thesis.AddReview(thesis.Topic.Supervisor);

        var sut = () => thesis.AddReview(thesis.Topic.Supervisor);

        sut.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void WhenTutorProvided_ShouldAddReview()
    {
        var thesis = FakeDataGenerator.GenerateThesis();
        var tutor = FakeDataGenerator.GenerateTutor();
        
        thesis.AddReview(tutor);
    
        thesis.Reviews.Should().HaveCount(1);
        thesis.Reviews.First().Reviewer.Should().Be(tutor);
    }
}