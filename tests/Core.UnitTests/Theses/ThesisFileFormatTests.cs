using Core.Models.Theses.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Theses;

public class ThesisFileFormatTests
{
    [Fact]
    public void ToString_WhenCalled_ShouldReturnName()
    {
        var fileFormat = ThesisFileFormat.Pdf;

        var toStringResult = fileFormat.ToString();

        toStringResult.Should().Be(fileFormat.Name);
    }
}