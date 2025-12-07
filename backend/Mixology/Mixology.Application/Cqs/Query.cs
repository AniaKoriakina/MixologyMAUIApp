using MediatR;
using Mixology.Application.Cqs.Interfaces;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Cqs;

public abstract class Query<TResult> : IRequest<Result<TResult>>, IQuery<Result<TResult>>
{
    
}