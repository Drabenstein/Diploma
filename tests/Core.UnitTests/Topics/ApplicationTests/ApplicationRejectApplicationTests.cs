using Core.Models.Topics.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.ApplicationTests
{
    public class ApplicationRejectApplicationTests : ApplicationsTestBase
    {
        [Fact]
        public void WhenStatusIsSent_ShouldChangeStatus()
        {
            application.RejectApplication();

            application.Status.Should().Be(ApplicationStatus.Rejected);
        }

        [Fact]
        public void WhenStatusIsNotSent_ShouldThrow()
        {
            application.CancelApplication();

            var sut = () => application.RejectApplication();

            sut.Should().Throw<InvalidOperationException>();
        }
    }
}
