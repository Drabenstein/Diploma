using Core.SeedWork;

namespace Core.Models.Theses;

public record Declaration : EntityBase
{
    public string ObjectiveOfWork { get; set; }
    public string OperatingRange { get; set; }
    public string Language { get; set; }
    public DateOnly Date { get; set; }
}