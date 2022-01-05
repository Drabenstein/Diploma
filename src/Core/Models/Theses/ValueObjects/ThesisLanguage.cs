namespace Core.Models.Theses.ValueObjects;

public record ThesisLanguage
{
    public static readonly ThesisLanguage Polish = new ThesisLanguage("PL");
    public static readonly ThesisLanguage English = new ThesisLanguage("EN");
    
    private ThesisLanguage(string language)
    {
        Language = language;
    }
    
    public string Language { get; }

    public override string ToString()
    {
        return Language;
    }
}