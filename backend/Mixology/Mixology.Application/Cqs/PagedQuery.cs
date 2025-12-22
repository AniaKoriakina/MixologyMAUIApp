using MediatR;
using Mixology.Application.Cqs.Interfaces;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Cqs;

public abstract class PagedQuery<TResult> : IRequest<Result<PagedResult<TResult>>>, IQuery<Result<PagedResult<TResult>>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    
    public int Skip => (Page - 1) * PageSize;
    public int Take => PageSize;
}