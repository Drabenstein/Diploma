using Application.Common;

namespace Application.Queries.Dtos;

public class ReviewDataDto
{
    public long ReviewId { get; set; }
    public long ThesisId { get; set; }
    public string Realizer { get; set; }
    public string Supervisor { get; set; }
    public string Topic { get; set; }
    public IEnumerable<ReviewModuleDto> Modules {get; set;}
}

