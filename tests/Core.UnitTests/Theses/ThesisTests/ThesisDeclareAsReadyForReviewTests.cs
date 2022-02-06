using Core.Models.Theses;
using Core.Models.Theses.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Theses.ThesisTests;

public class ThesisDeclareAsReadyForReviewTests
{
    [Fact]
    public void WhenStatusInProgress_ShouldSetStatusReadyToReview()
    {
        var thesis = FakeDataGenerator.GenerateThesis();
        
        thesis.DeclareAsReadyForReview();

        thesis.Status.Should().Be(ThesisStatus.ReadyToReview);
    }

    [Fact]
    public void WhenStatusIsNotInProgress_ShouldThrow()
    {
        var thesis = FakeDataGenerator.GenerateThesis();
        thesis.SetPropertyValue(nameof(Thesis.Status), ThesisStatus.AddedToAsap);
        
        Action sut = () => thesis.DeclareAsReadyForReview();

        sut.Should().Throw<InvalidOperationException>();
    }
}