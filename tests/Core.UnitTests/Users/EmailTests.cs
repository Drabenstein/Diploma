using Core.Models.Users.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Users;

public class EmailTests
{
    [Fact]
    public void CreateStandardTutorEmail_WhenFirstAndLastNameProvided_ShouldReturnTutorEmail()
    {
        var email = Email.CreateStandardTutorEmail("Jan", "Kowalski");

        email.Address.Should().Be("jan.kowalski@pwr.edu.pl");
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void CreateStandardTutorEmail_WhenFirstNameIsWhitespace_ShouldThrow(string firstName)
    {
        const string lastName = "Kowalski";

        var sut = () => Email.CreateStandardTutorEmail(firstName, lastName);

        sut.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void CreateStandardTutorEmail_WhenLastNameIsWhitespace_ShouldThrow(string lastName)
    {
        const string firstName = "Jan";

        var sut = () => Email.CreateStandardTutorEmail(firstName, lastName);

        sut.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void CreateStudentEmail_WhenIndexProvided_ShouldReturnStudentEmail()
    {
        var email = Email.CreateStudentEmail(458123);

        email.Address.Should().Be("458123@student.pwr.edu.pl");
    }

    [Fact]
    public void StringCast_WhenRequested_ShouldReturnAddressString()
    {
        const string emailString = "jan.kowalski@pwr.edu.pl";
        var email = new Email(emailString);

        string castedString = email;

        castedString.Should().Be(emailString);
    }
}