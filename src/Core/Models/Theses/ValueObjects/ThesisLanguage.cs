namespace Core.Models.Theses.ValueObjects;

public record ThesisLanguage
{
    public static ThesisLanguage Polish { get; } = new ThesisLanguage("PL");
    public static ThesisLanguage English { get; } = new ThesisLanguage("EN");
    
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