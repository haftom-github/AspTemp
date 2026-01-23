using AspTemp.Shared.ResultContracts;
using MediatR;
using Unit = AspTemp.Shared.Domain.Unit;

namespace AspTemp.Shared.Cqrs;

public interface IRRequest : IRequest<Result<Unit>>;
public interface IRRequest<TResponse> : IRequest<Result<TResponse>>;

public interface IGetManyRequest<TResponse>
    : IRRequest<IEnumerable<TResponse>>
{
    string? Search { get; set; }
    string? OrderBy { get; set; }
    bool Descending { get; set; }
}

public record PaginatedRequest<TResponse> : IGetManyRequest<TResponse>
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; } = null;
    public string? OrderBy { get; set; } = null;
    public bool Descending { get; set; } = false;
}

public interface IRRequestHandler<in TRequest> 
    : IRequestHandler<TRequest, Result<Unit>>
    where TRequest : IRRequest;

public interface IRRequestHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : IRRequest<TResponse>;
