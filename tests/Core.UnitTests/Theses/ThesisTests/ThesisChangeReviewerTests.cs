using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Theses.ThesisTests;

public class ThesisChangeReviewerTests
{
    [Fact]
    public void WhenIsAlreadyReviewed_ShouldThrow()
    {
        var thesis = FakeDataGenerator.GenerateThesis();
        var reviewer = FakeDataGenerator.GenerateTutor();
        thesis.AddReview(reviewer);
        thesis.DeclareAsReadyForReview();
        thesis.ReviewThesis("3", thesis.Reviews.First().Id);
        var anotherReviewer = FakeDataGenerator.GenerateTutor();

        var sut = () => thesis.ChangeReviewer(anotherReviewer);

        sut.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void WhenNoReviewerIsPresent_ShouldAddReview()
    {
        var thesis = FakeDataGenerator.GenerateThesis();
        thesis.AddReview(thesis.Topic.Supervisor);
        var reviewer = FakeDataGenerator.GenerateTutor();
        
        thesis.ChangeReviewer(reviewer);

        thesis.Reviews.Should().HaveCount(2);
        thesis.Reviews.Last().Reviewer.Should().Be(reviewer);
    }

    [Fact]
    public void WhenReviewerExists_ShouldChangeToNewReviewer()
    {
        var thesis = FakeDataGenerator.GenerateThesis();
        thesis.AddReview(thesis.Topic.Supervisor);
        var oldReviewer = FakeDataGenerator.GenerateTutor();
        thesis.AddReview(oldReviewer);
        thesis.Reviews.Last().SetId(1);
        var newReviewer = FakeDataGenerator.GenerateTutor();

        thesis.ChangeReviewer(newReviewer);

        thesis.Reviews.Should().HaveCount(2);
        thesis.Reviews.Last().Id.Should().Be(0);
        thesis.Reviews.Last().Reviewer.Should().Be(newReviewer);
    }

    [Fact]
    public void WhenNewReviewerIsTheSameAsOldReviewer_ShouldDoNothing()
    {
        var thesis = FakeDataGenerator.GenerateThesis();
        thesis.AddReview(thesis.Topic.Supervisor);
        var reviewer = FakeDataGenerator.GenerateTutor();
        thesis.AddReview(reviewer);
        thesis.Reviews.Last().SetId(1);

        thesis.ChangeReviewer(reviewer);

        thesis.Reviews.Should().HaveCount(2);
        thesis.Reviews.Last().Id.Should().Be(1);
        thesis.Reviews.Last().Reviewer.Should().Be(reviewer);
    }
}