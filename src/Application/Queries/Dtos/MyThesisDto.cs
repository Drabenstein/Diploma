namespace Application.Queries.Dtos;

public class MyThesisDto
{
    public long Id { get; set; }
    public string Topic { get; set; }
    public string Status { get; set; }
    public IEnumerable<ReviewForMyThesisDto> Reviews { get; set; }
}

