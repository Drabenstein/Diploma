using MediatR;

namespace Application.Common;

public interface IPagedRequest<T> : IRequest<PagedResultDto<T>>
{
    public long Page { get; }
    public long ItemsPerPage { get; }
}