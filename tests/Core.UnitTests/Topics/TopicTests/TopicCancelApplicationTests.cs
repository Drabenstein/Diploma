using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Topics;
using Core.Models.Users;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Topics.TopicTests
{
    public class TopicCancelApplicationTests
    {
        private readonly Topic topic;
        private readonly Application application;
        private readonly Student student;

        public TopicCancelApplicationTests()
        {
            topic = new Topic()
            {
                Name = "Informatyka stosowana",
                EnglishName = "Applied computer science",
                IsFree = true,
                Proposer = FakeDataGenerator.GenerateUser(),
                IsProposedByStudent = false,
                MaxRealizationNumber = 4,
                FieldOfStudy = FakeDataGenerator.GenerateFieldOfStudy(),
                Supervisor = FakeDataGenerator.GenerateTutor(),
                YearOfDefence = "2021/2022"
            };
            student = FakeDataGenerator.GenerateStudent();
            application = new Application()
            {
                Topic = topic,
                IsTopicProposal = false,
                Message = "Test message",
                Submitter = student,
                Timestamp = new DateTime(2022, 01, 10)
            };
        }

        [Fact]
        public void WhenApplicationExists_ShouldRemoveIt()
        {

        }
    }
}
