using Core.Models.Reviews;
using Core.Models.Reviews.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Reviews;

public class ReviewSubmitGradeTests
{
    [Theory]
    [InlineData("2", Grade.Two)]
    [InlineData("3", Grade.Three)]
    [InlineData("3.5", Grade.ThreeAndHalf)]
    [InlineData("4", Grade.Four)]
    [InlineData("4.5", Grade.FourAndHalf)]
    [InlineData("5", Grade.Five)]
    [InlineData("5.5", Grade.FiveAndHalf)]
    public void WhenGradeIsValid_ShouldAssignIt(string gradeText, Grade expectedGrade)
    {
        var tutor = FakeDataGenerator.GenerateTutor();
        var review = new Review(tutor);
        
        review.SubmitGrade(gradeText);

        review.Grade.Should().Be(expectedGrade);
    }

    [Theory]
    [InlineData("eeedsae")]
    [InlineData("6")]
    [InlineData("1")]
    [InlineData("0")]
    public void WhenGradeIsInvalidString_ShouldThrow(string grade)
    {
        var tutor = FakeDataGenerator.GenerateTutor();
        var review = new Review(tutor);
        
        var sut = () => review.SubmitGrade(grade);

        sut.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void WhenReviewIsAlreadyGraded_ShouldThrow()
    {
        var tutor = FakeDataGenerator.GenerateTutor();
        var review = new Review(tutor);
        review.SubmitGrade("3");
        
        var sut = () => review.SubmitGrade("4");

        sut.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void WhenReviewIsGraded_ShouldSetPublishTimestamp()
    {
        var tutor = FakeDataGenerator.GenerateTutor();
        var review = new Review(tutor);
        
        review.SubmitGrade("3");

        review.PublishTimestamp.Should().NotBeNull();
    }

    [Fact]
    public void WhenReviewIsGraded_ShouldSetIsPublishedFlag()
    {
        var tutor = FakeDataGenerator.GenerateTutor();
        var review = new Review(tutor);
        
        review.SubmitGrade("3");

        review.IsPublished.Should().BeTrue();
    }
}