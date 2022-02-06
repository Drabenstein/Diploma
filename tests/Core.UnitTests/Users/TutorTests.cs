using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Users;
using Core.Models.Users.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Core.UnitTests.Users
{
    public class TutorTests
    {
        [Fact]
        public void Constructor_WhenCalledBasicVersion_ShouldSetDefault()
        {
            var tutor = new Tutor("Jan", "Kowalski", new Email("jan.kowalski@pwr.edu.pl"));


            tutor.Department.Should().BeEmpty();
            tutor.AcademicDegree.Should().Be(AcademicDegree.Engineer);
            tutor.Pensum.Should().Be(0);
            tutor.Position.Should().Be(TutorPosition.Lecturer);
            tutor.IsPositiveFacultyOpinion.Should().BeFalse();
        }
    }
}
