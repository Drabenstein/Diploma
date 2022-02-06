using Core.Models.Theses;
using Core.Models.Theses.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Theses.ThesisTests;

public class ThesisReviewThesisTests
{
    [Fact]
    public void WhenNotReadyToReviewNorReviewed_ShouldThrow()
    {
        var thesis = FakeDataGenerator.GenerateThesis();
        thesis.AddReview(thesis.Topic.Supervisor);
        
        var sut = () => thesis.ReviewThesis("3", 0);

        sut.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void WhenReviewed_ShouldChangeStatus()
    {
        var thesis = FakeDataGenerator.GenerateThesis();
        thesis.AddReview(thesis.Topic.Supervisor);
        thesis.DeclareAsReadyForReview();
        
        thesis.ReviewThesis("3", thesis.Reviews.First().Id);

        thesis.Status.Should().Be(ThesisStatus.Reviewed);
    }
    
    [Fact]
    public void WhenReviewNotFound_ShouldThrow()
    {
        var thesis = FakeDataGenerator.GenerateThesis();
        thesis.DeclareAsReadyForReview();
        
        var sut = () => thesis.ReviewThesis("3", 0);

        sut.Should().Throw<InvalidOperationException>();
    }
}