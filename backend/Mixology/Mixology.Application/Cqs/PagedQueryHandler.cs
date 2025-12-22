using MediatR;
using Mixology.Application.Cqs.Interfaces;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Cqs;

public abstract class PagedQueryHandler<TQuery, TResult> : HandleBase<TQuery, Result<PagedResult<TResult>>>, IQueryHandler<TQuery, Result<PagedResult<TResult>>> 
    where TQuery : PagedQuery<TResult>
{
    protected Result Success() => Result.Success();
    protected Result<PagedResult<TResult>> Success(PagedResult<TResult> result) => Result<PagedResult<TResult>>.Success(result);
    protected Result<PagedResult<TResult>> Error(IError error) => Result<PagedResult<TResult>>.Error(error);
}