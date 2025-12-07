using MediatR;
using Mixology.Application.Cqs.Interfaces;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Cqs;

public abstract class Command : IRequest<Result>, ICommand
{
    
}