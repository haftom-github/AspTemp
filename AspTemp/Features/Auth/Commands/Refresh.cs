using AspTemp.Features.Auth.Services;
using AspTemp.Shared.Application.Contracts.Cqrs;
using AspTemp.Shared.Application.Contracts.ResultContracts;

namespace AspTemp.Features.Auth.Commands;

public record Refresh(string RefreshToken)
    : IRRequest<(string accessToken, string refreshToken)>;

public class RefreshHandler(ITokenService tokenService)
    : IRRequestHandler<Refresh, (string accessToken, string refreshToken)>
{
    public async Task<Result<(string accessToken, string refreshToken)>> 
        Handle(Refresh request, CancellationToken cancellationToken)
    {
        var tokens = await tokenService.Refresh(request.RefreshToken);
        if (tokens == null) return Failure.Unauthorized();
        return tokens;
    }
}