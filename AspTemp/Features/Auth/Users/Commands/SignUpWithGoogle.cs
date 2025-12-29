using AspTemp.Features.Auth.AuthProviders.Domain;
using AspTemp.Features.Auth.Users.Domain;
using AspTemp.Shared.Application;
using AspTemp.Shared.Application.Contracts.Cqrs;
using AspTemp.Shared.Application.Contracts.ResultContracts;
using AspTemp.Shared.Domain;
using Google.Apis.Auth;

namespace AspTemp.Features.Auth.Users.Commands;

public record SignUpWithGoogle(
    string IdToken
) : IRRequest;

public class SignUpWithGoogleHandler(
    IAuthProviderRepo authProviderRepo, 
    IUserRepo userRepo, 
    IUnitOfWork unitOfWork)
    : IRRequestHandler<SignUpWithGoogle>
{
    public async Task<Result<Unit>> Handle(SignUpWithGoogle request, CancellationToken cancellationToken)
    {
        var googleAuthProvider = await authProviderRepo.GetByNameAsync("google");
        if (googleAuthProvider == null)
        {
            Console.WriteLine("\n---------google-auth-provider-not-found----------\n");
            return Failure.Unexpected();
        }
        
        var settings = new GoogleJsonWebSignature
            .ValidationSettings { Audience = [googleAuthProvider.ClientId!] };
        
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
        var googleUserId = payload.Subject!;

        if (await userRepo.AuthIdentityExistsAsync(googleAuthProvider.Id, googleUserId, cancellationToken)
            || await userRepo.EmailExistsAsync(payload.Email, cancellationToken))
            return Failure.Conflict("this google account is already linked to another user");

        var user = new User { Id = Guid.NewGuid(), Email = payload.Email };
        
        user.AddAuthIdentity(googleAuthProvider.Id, googleUserId);
        
        await userRepo.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new Unit();
    }
}

// code challenge = R1_zVaosxO3yVIOSU31IG53VfwocZvNDT04m1nxI8II
// code verifier = pzF0qJe6ubi85cIJXRSQeOJvwWrfqF9yn22Cfs0J6Pv-RNlO2h~F_FIio6MEFQ-rG8Fwq7yvA.754o9D~ZLeWq7b~EL5gCo3RKIr81tpK8GDM6.kTwuOox~Pexc0tYc7
// https://accounts.google.com/o/oauth2/v2/auth?client_id=141908131732-akm8g7605c0bcdj5ns371h0g9ggobkcs.apps.googleusercontent.com&redirect_uri=https://localhost:3000&response_type=code&scope=openid%20email%20profile&code_challenge=R1_zVaosxO3yVIOSU31IG53VfwocZvNDT04m1nxI8II&code_challenge_method=S256