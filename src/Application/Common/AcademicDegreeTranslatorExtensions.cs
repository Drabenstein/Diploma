using Core.Models.Users.ValueObjects;

namespace Application.Common;

public static class AcademicDegreeTranslatorExtensions
{
    public static string Translate(this string? title)
    {
        if (title is null)
        {
            return string.Empty;
        }

        return title switch
        {
            "Engineer" => "inż.",
            "MasterOfScience" => "mgr",
            "MasterOfScienceBEng" => "mgr inż.",
            "DoctorOfPhilosophy" => "dr",
            "DoctorOfPhilosophyBEng" => "dr inż.",
            "DoctorOfScience" => "dr hab.",
            "DoctorOfScienceBEng" => "dr hab. inż.",
            "ProfessorOfUniversity" => "prof. dr hab.",
            "Professor" => "prof. dr hab. inż.",
            _ => throw new ArgumentOutOfRangeException(nameof(title))
        };
    }

    public static string CombineAcademicDegreeAndFullName(this string? fullName, string? academicTitle)
    {
        if (fullName is null)
        {
            return string.Empty;
        }

        if (academicTitle is null)
        {
            return fullName;
        }

        return $"{academicTitle.Translate()} {fullName}";
    }
}