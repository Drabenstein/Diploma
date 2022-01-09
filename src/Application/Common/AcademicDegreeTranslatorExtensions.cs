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
            "AcademicDegree.Engineer" => "inż.",
            "AcademicDegree.MasterOfScience" => "mgr",
            "AcademicDegree.MasterOfScienceBEng" => "mgr inż.",
            "AcademicDegree.DoctorOfPhilosophy" => "dr",
            "AcademicDegree.DoctorOfPhilosophyBEng" => "dr inż.",
            "AcademicDegree.DoctorOfScience" => "dr hab.",
            "AcademicDegree.DoctorOfScienceBEng" => "dr hab. inż.",
            "AcademicDegree.ProfessorOfUniversity" => "prof. dr hab.",
            "AcademicDegree.Professor" => "prof. dr hab. inż.",
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