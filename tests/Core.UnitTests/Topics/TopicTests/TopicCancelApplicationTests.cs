using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Topics;
using Core.Models.Topics.ValueObjects;
using Core.Models.Users;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.TopicTests
{
    public class TopicCancelApplicationTests : TopicTestBase
    {
        [Fact]
        public void WhenApplicationExists_ShouldCancelIt()
        {
            topic.SubmitApplication(application);
            
            topic.CancelApplication(application.Id);

            topic.Applications.First().Status.Should().Be(ApplicationStatus.Cancelled);
        }

        [Fact]
        public void WhenApplicationDoesNotExist_ShouldThrow()
        {
            Action sut = () => topic.CancelApplication(application.Id);

            sut.Should().Throw<Exception>();
        }

        [Fact]
        public void WhenApplicationIsAlreadyConfirmed_ShouldThrow()
        {
            topic.SubmitApplication(application);
            topic.AcceptApplication(application.Id);
            topic.ConfirmApplication(application.Id);

            Action sut = () => topic.CancelApplication(application.Id);

            sut.Should().Throw<InvalidOperationException>();
        }
    }
}
