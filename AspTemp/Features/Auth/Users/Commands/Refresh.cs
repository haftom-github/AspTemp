using AspTemp.Features.Auth.Users.Services;
using AspTemp.Shared.Application.Contracts.Cqrs;
using AspTemp.Shared.Application.Contracts.ResultContracts;

namespace AspTemp.Features.Auth.Users.Commands;

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