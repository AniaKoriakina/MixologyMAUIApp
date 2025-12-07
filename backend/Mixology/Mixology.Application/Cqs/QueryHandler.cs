using MediatR;
using Mixology.Application.Cqs.Interfaces;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Cqs;

public abstract class QueryHandler<TQuery, TResult> : HandleBase<TQuery, Result<TResult>>, IQueryHandler<TQuery, Result<TResult>> 
    where TQuery : Query<TResult>
{
    protected Result Success() => Result.Success();
    protected Result<TResult> Success(TResult result) => Result<TResult>.Success(result);
    protected Result<TResult> Error(IError error) => Result<TResult>.Error(error);
}