namespace Application.Commands.Dtos;

public class SendDeclarationDto
{
    public int ThesisId { get; set; }
    public string ObjectiveOfWork { get; set; }
    public string OperatingRange { get; set; }
    public string Language { get; set; }
    public bool HasConsentToChangeLanguage { get; set; }
    public DateTime DeclarationDateTime { get; set; }
}
