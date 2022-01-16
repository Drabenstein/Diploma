namespace Application.Common;

public class PagedResultDto<T>
{
    public int CurrentPage { get; set; }
    public int TotalItems { get; set; }
    public bool HasNextPage { get; set; }
    public IEnumerable<T> Results { get; set; } = null!;
}