﻿using System.Diagnostics.CodeAnalysis;
using Core.Models.Users.ValueObjects;

namespace Core.Models.Users;

public record Tutor : User
{
    // EF Core only
    [ExcludeFromCodeCoverage]
    private Tutor() { }
    
    public Tutor(string firstName, string lastName, Email email, int pensum,
        TutorPosition position, string department, AcademicDegree academicDegree)
        : base(firstName, lastName, email)
    {
        Pensum = pensum;
        Position = position;
        HasConsentToExtendPensum = false;
        Department = department;
        AcademicDegree = academicDegree;
        IsPositiveFacultyOpinion = false;
    }
    
    public Tutor(string firstName, string lastName, Email email)
        : base(firstName, lastName, email)
    {
        Pensum = 0;
        Position = TutorPosition.Lecturer;
        HasConsentToExtendPensum = false;
        Department = "";
        AcademicDegree = AcademicDegree.Engineer;
        IsPositiveFacultyOpinion = false;
    }

    public int Pensum { get; set; }
    public TutorPosition Position { get; set; }
    public bool HasConsentToExtendPensum { get; set; }
    public string Department { get; set; }
    public AcademicDegree AcademicDegree { get; set; }
    public bool IsPositiveFacultyOpinion { get; set; }
}