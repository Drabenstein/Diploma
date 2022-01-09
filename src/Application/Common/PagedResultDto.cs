namespace Application.Common;

public class PagedResultDto<T>
{
    public long CurrentPage { get; set; }
    public long TotalItems { get; set; }
    public bool HasNextPage { get; set; }
    public IEnumerable<T> Results { get; set; }
}