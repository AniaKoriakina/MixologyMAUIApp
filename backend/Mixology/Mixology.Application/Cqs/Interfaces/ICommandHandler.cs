using Mixology.Core.Shared.Result;

namespace Mixology.Application.Cqs.Interfaces;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
}