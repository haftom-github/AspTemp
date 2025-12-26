using AspTemp.Features.Auth.Services;
using AspTemp.Shared.Application.Contracts.Cqrs;
using AspTemp.Shared.Application.Contracts.ResultContracts;
using Google.Apis.Auth;

namespace AspTemp.Features.Auth.Commands;

public record SignInWithGoogle(string IdToken): IRRequest<Tokens>;

public class SignInWithGoogleHandler(IUserRepo userRepo, ITokenService tokenService, IConfiguration config)
    : IRRequestHandler<SignInWithGoogle, Tokens>
{
    public async Task<Result<Tokens>> Handle(SignInWithGoogle request, CancellationToken cancellationToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [config["OAuth:Google:ClientId"]!]
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
        var user = await userRepo.GetByEmailAsync(payload.Email);
        if (user == null)
            return Failure.Validation("User with google identifier not found.");
        
        return tokenService.GenerateTokens(user);
    }
}