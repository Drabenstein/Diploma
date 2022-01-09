using MediatR;

namespace Application.Common;

public interface IPagedRequest<T> : IRequest<PagedResultDto<T>>
{
    public long Page { get; init; }
    public long ItemsPerPage { get; init; }
}