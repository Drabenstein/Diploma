using Core.Models.Topics.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.ApplicationTests
{
    public class ApplicationConfirmApplicationTests : ApplicationsTestBase
    {
        [Fact]
        public void WhenStatusIsApproved_ShouldChangeStatus()
        {
            application.AcceptApplication();
            
            application.ConfirmApplication();

            application.Status.Should().Be(ApplicationStatus.Confirmed);
        }

        [Fact]
        public void WhenStatusIsNotApproved_ShouldThrow()
        {
            var sut = () => application.ConfirmApplication();

            sut.Should().Throw<InvalidOperationException>();
        }
    }
}
