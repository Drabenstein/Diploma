using Core.Models.Users;
using Core.Models.Users.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Users
{
    public class UserTests
    {
        [Fact]
        public void CreatedUser_ShouldHaveAllPropertiesSet()
        {
            string firstName = "Jan";
            string lastName = "Kowalski";
            var email = new Email("j.kowalski@example.com");

            var user = new User(firstName, lastName, email);

            user.Id.Should().Be(0);
            user.Email.Should().Be(email);
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
            user.AreasOfInterest.Should().BeEmpty();
        }

        [Fact]
        public void AddAreaOfInterest_WhenNewAreaIsAdded_ShouldSaveThatArea()
        {
            var user = new User("Jan", "Kowalski", new Email("j.kowalski@example.com"));
            var areaOfInterest = new AreaOfInterest
            {
                Name = "Area no. 1"
            };

            user.AddAreaOfInterest(areaOfInterest);

            user.AreasOfInterest.Should().HaveCount(1);
            user.AreasOfInterest.First().Should().Be(areaOfInterest);
        }

        [Fact]
        public void AddAreaOfInterest_WhenNewAreaIsAdded_ShouldRegisterUserWithArea()
        {
            var user = new User("Jan", "Kowalski", new Email("j.kowalski@example.com"));
            var areaOfInterest = new AreaOfInterest
            {
                Name = "Area no. 1"
            };

            user.AddAreaOfInterest(areaOfInterest);

            areaOfInterest.Users.Should().HaveCount(1);
            areaOfInterest.Users.First().Should().Be(user);
        }

        [Fact]
        public void RemoveAreaOfInterest_WhenAreaIsRemoved_ShouldRemoveThatArea()
        {
            var user = new User("Jan", "Kowalski", new Email("j.kowalski@example.com"));
            var areaOfInterest = new AreaOfInterest
            {
                Name = "Area no. 1"
            };
            user.AddAreaOfInterest(areaOfInterest);

            user.RemoveAreaOfInterest(areaOfInterest);

            user.AreasOfInterest.Should().BeEmpty();
        }

        [Fact]
        public void RemoveAreaOfInterest_WheAreaIsRemoved_ShouldUnregisterUserFromArea()
        {
            var user = new User("Jan", "Kowalski", new Email("j.kowalski@example.com"));
            var areaOfInterest = new AreaOfInterest
            {
                Name = "Area no. 1"
            };
            user.AddAreaOfInterest(areaOfInterest);

            user.RemoveAreaOfInterest(areaOfInterest);

            areaOfInterest.Users.Should().BeEmpty();
        }
    }
}
