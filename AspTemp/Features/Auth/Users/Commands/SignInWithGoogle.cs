using AspTemp.Features.Auth.AuthProviders.Domain;
using AspTemp.Features.Auth.Users.Services;
using AspTemp.Shared.Application.Contracts.Cqrs;
using AspTemp.Shared.Application.Contracts.ResultContracts;
using Google.Apis.Auth;

namespace AspTemp.Features.Auth.Users.Commands;

public record SignInWithGoogle(string IdToken): IRRequest<Tokens>;

public class SignInWithGoogleHandler(
    IUserRepo userRepo, 
    IAuthProviderRepo authProviderRepo, 
    ITokenService tokenService, 
    IConfiguration config
    ) : IRRequestHandler<SignInWithGoogle, Tokens>
{
    public async Task<Result<Tokens>> Handle(SignInWithGoogle request, CancellationToken cancellationToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [config["OAuth:Google:ClientId"]!]
        };

        var googleAuthProvider = await authProviderRepo.GetByNameAsync("google");
        if (googleAuthProvider == null)
        {
            Console.WriteLine("\n---------google-auth-provider-not-found----------\n");
            return Failure.Unexpected();
        }
        
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
        var googleUserId = payload.Subject!;
        
        var user = await userRepo.GetByAuthProviderAsync(googleAuthProvider.Id, googleUserId, cancellationToken);
        if (user == null)
            return Failure.Validation("User with google identifier not found.");
        
        return tokenService.GenerateTokens(user);
    }
}