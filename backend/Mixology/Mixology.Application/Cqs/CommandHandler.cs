using Mixology.Application.Cqs.Interfaces;
using Mixology.Core.Shared.Result;

namespace Mixology.Application.Cqs;

public abstract class CommandHandler<TCommand> : HandleBase<TCommand, Result>, ICommandHandler<TCommand> 
    where TCommand : Command
{
    protected Result Success() => Result.Success();
    protected Result Error(IError error) => Result.Error(error);
}