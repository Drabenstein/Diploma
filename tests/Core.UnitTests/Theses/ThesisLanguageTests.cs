using Core.Models.Theses.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Theses;

public class ThesisLanguageTests
{
    [Fact]
    public void ToString_WhenCalled_ShouldReturnLanguageLocale()
    {
        var language = ThesisLanguage.English;

        var toStringResult = language.ToString();

        toStringResult.Should().Be(language.Language);
    }
}