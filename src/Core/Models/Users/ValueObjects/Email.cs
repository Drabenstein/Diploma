using System.Globalization;
using System.Text.RegularExpressions;

namespace Core.Models.Users.ValueObjects;

public record Email
{
    private const int RegexTimeoutSeconds = 2;
    private const string EmailRegexPattern = @"[a-zA-Z0-9-_.]+@[a-z0-9-]*\.?pwr\.edu\.pl";

    private static readonly Regex EmailRegex = new Regex(EmailRegexPattern,
        RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase,
        TimeSpan.FromSeconds(RegexTimeoutSeconds));

    public Email(string address)
    {
        if (!EmailRegex.IsMatch(address))
        {
            throw new ArgumentException($"{address} is not a valid e-mail from pwr.edu.pl domain", nameof(address));
        }

        Address = address;
    }

    public string Address { get; }

    public static Email CreateStudentEmail(int indexNumber)
    {
        return new Email($"{indexNumber.ToString(CultureInfo.InvariantCulture)}@student.pwr.edu.pl");
    }

    public static Email CreateStandardTutorEmail(string firstName, string lastName)
    {
        return new Email($"{firstName}.{lastName}@student.pwr.edu.pl");
    }
}