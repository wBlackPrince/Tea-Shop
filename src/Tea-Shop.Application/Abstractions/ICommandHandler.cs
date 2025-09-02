using CSharpFunctionalExtensions;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Abstractions;

public interface ICommand;

public interface ICommandHandler<TResponse, in TCommand>
    where TCommand : ICommand
{
    Task<Result<TResponse, Error>> Handle(
        TCommand command,
        CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<UnitResult<Error>> Handle(
        TCommand command,
        CancellationToken cancellationToken);
}