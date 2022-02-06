using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Users;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Users
{
    public  class AreaOfInterestTests
    {
        private readonly AreaOfInterest areaOfInterest = new AreaOfInterest
        {
            Name = "Computer science"
        };

        [Fact]
        public void RemoveUser_WhenUserIsPresent_ShouldRemove()
        {            
            var user = FakeDataGenerator.GenerateUser();
            areaOfInterest.AddUser(user);

            areaOfInterest.RemoveUser(user);

            areaOfInterest.Users.Should().BeEmpty();
        }

        [Fact]
        public void RemoveUser_WhenUserNotFound_ShouldDoNothing()
        {
            var user = FakeDataGenerator.GenerateUser();

            areaOfInterest.RemoveUser(user);

            areaOfInterest.Users.Should().BeEmpty();
        }

        [Fact]
        public void AddUser_WhenUserAdded_ShouldSave()
        {
            var user = FakeDataGenerator.GenerateUser();

            areaOfInterest.AddUser(user);

            areaOfInterest.Users.Should().HaveCount(1);
            areaOfInterest.Users.First().Should().Be(user);
        }

        [Fact]
        public void AddUser_WhenUserAlreadyIsAdded_ShouldThrow()
        {
            var user = FakeDataGenerator.GenerateUser();
            areaOfInterest.AddUser(user);

            var sut = () => areaOfInterest.AddUser(user);

            sut.Should().Throw<InvalidOperationException>();
        }
    }
}
