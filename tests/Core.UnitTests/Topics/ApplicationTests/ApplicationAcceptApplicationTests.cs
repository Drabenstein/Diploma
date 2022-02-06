using Core.Models.Topics.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.ApplicationTests
{
    public class ApplicationAcceptApplicationTests : ApplicationsTestBase
    {
        [Fact]
        public void WhenStatusIsSent_ShouldChangeStatus()
        {
            application.AcceptApplication();

            application.Status.Should().Be(ApplicationStatus.Approved);
        }

        [Fact]
        public void WhenStatusIsNotSent_ShouldThrow()
        {
            application.CancelApplication();

            var sut = () => application.AcceptApplication();

            sut.Should().Throw<InvalidOperationException>();
        }
    }
}
