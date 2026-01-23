using AspTemp.Features.Auth.Services;
using AspTemp.Shared.Cqrs;
using AspTemp.Shared.ResultContracts;

namespace AspTemp.Features.Auth.Commands;

public record Refresh(string RefreshToken)
    : IRRequest<Tokens>;

public class RefreshHandler(ITokenService tokenService)
    : IRRequestHandler<Refresh, Tokens>
{
    public async Task<Result<Tokens>> 
        Handle(Refresh request, CancellationToken cancellationToken)
    {
        var tokens = await tokenService.Refresh(request.RefreshToken, cancellationToken);
        if (tokens == null) return Failure.Unauthorized();

        return tokens;
    }
}