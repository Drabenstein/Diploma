﻿using System.Diagnostics.CodeAnalysis;

namespace Core.Models.Theses.ValueObjects;

public record ThesisFileFormat
{
    public static readonly ThesisFileFormat Pdf = new ThesisFileFormat("pdf");

    // EF-Core only
    [ExcludeFromCodeCoverage]
    private ThesisFileFormat()
    {
    }
    
    private ThesisFileFormat(string name)
    {
        Name = name;
    }
    
    public string Name { get; }

    public override string ToString()
    {
        return Name;
    }
}