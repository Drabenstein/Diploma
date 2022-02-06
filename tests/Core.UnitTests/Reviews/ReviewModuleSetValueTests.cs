using System.Globalization;
using Core.Models.Reviews;
using Core.Models.Reviews.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Reviews;

public class ReviewModuleSetValueTests
{
    [Fact]
    public void WhenValueIsText_ShouldSetValue()
    {
        var reviewModule = FakeDataGenerator.GenerateReviewModule(ReviewModuleType.Text);
        const string newValue = "Test value";
        
        reviewModule.SetValue(newValue);

        reviewModule.Value.Should().Be(newValue);
    }

    [Fact]
    public void WhenValueIsNumber_ShouldSetValue()
    {
        var reviewModule = FakeDataGenerator.GenerateReviewModule(ReviewModuleType.Number);
        var newValue = 3.5.ToString(CultureInfo.InvariantCulture);
        
        reviewModule.SetValue(newValue);

        reviewModule.Value.Should().Be(newValue);
    }

    [Fact]
    public void WhenValueIsInvalidNumber_ShouldThrow()
    {
        var reviewModule = FakeDataGenerator.GenerateReviewModule(ReviewModuleType.Number);
        var newValue = "312d";
        
        var sut = () => reviewModule.SetValue(newValue);

        sut.Should().Throw<InvalidOperationException>();
    }
}