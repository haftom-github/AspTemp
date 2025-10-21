using AspTemp.Shared.Application.Contracts.ResultContracts;
using MediatR;

namespace AspTemp.Shared.Application.Contracts.Cqrs;

// command with no response
public interface ICommand: IRequest<Result>;
public interface ICommandHandler<in TCommand>
    : IRequestHandler<TCommand, Result>
    where TCommand : ICommand;


// command with response
public interface ICommand<TResponse>
    : IRequest<Result<TResponse>>;
public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>;


// query with response
public interface IQuery<TResponse>
    : IRequest<Result<TResponse>>;
public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
