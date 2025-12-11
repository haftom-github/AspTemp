using AspTemp.Shared.Application.Contracts.ResultContracts;
using MediatR;
using Unit = AspTemp.Shared.Domain.Unit;

namespace AspTemp.Shared.Application.Contracts.Cqrs;

public interface IRRequest : IRequest<Result<Unit>>;
public interface IRRequest<TResponse> : IRequest<Result<TResponse>>;

public interface IRRequestHandler<in TRequest> 
    : IRequestHandler<TRequest, Result<Unit>>
    where TRequest : IRRequest;

public interface IRRequestHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : IRRequest<TResponse>;
    