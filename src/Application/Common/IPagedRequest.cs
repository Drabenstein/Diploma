using MediatR;

namespace Application.Common;

public interface IPagedRequest<T> : IRequest<PagedResultDto<T>>
{
    public int Page { get; }
    public int ItemsPerPage { get; }
}