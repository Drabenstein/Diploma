using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Topics;
using Core.Models.Topics.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.ApplicationTests
{
    public class ApplicationCancelApplicationTests : ApplicationsTestBase
    {
        [Fact]
        public void WhenStatusIsConfirmed_ShouldThrow()
        {
            application.AcceptApplication();
            application.ConfirmApplication();

            var sut = () => application.CancelApplication();

            sut.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(ApplicationStatus.Sent)]
        [InlineData(ApplicationStatus.Approved)]
        [InlineData(ApplicationStatus.Cancelled)]
        public void WhenStatusIsNotConfirmedNorRejected_ShouldChangeStatus(ApplicationStatus status)
        {
            application.SetPropertyValue(nameof(Application.Status), status);

            application.CancelApplication();

            application.Status.Should().Be(ApplicationStatus.Cancelled);
        }

        [Fact]
        public void WhenStatusIsRejected_ShouldDoNothing()
        {
            application.RejectApplication();

            application.CancelApplication();

            application.Status.Should().Be(ApplicationStatus.Rejected);
        }
    }
}
