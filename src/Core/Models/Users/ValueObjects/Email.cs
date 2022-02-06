using System.Globalization;
using System.Text.RegularExpressions;

namespace Core.Models.Users.ValueObjects;

public record Email(string Address)
{
    public static Email CreateStudentEmail(int indexNumber)
    {
        return new Email($"{indexNumber.ToString(CultureInfo.InvariantCulture)}@student.pwr.edu.pl");
    }

    public static Email CreateStandardTutorEmail(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentNullException(nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentNullException(nameof(lastName));
        }
        
        return new Email($"{firstName.ToLowerInvariant()}.{lastName.ToLowerInvariant()}@pwr.edu.pl");
    }

    public static implicit operator string(Email email)
    {
        return email.Address;
    }

}