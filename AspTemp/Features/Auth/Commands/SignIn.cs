using AspTemp.Features.Auth.Services;
using AspTemp.Shared.Application.Contracts.Cqrs;
using AspTemp.Shared.Application.Contracts.ResultContracts;

namespace AspTemp.Features.Auth.Commands;

public record SignIn(
    string UsernameOrEmail,
    string Password
): IRRequest<Tokens>;

public record Tokens(string AccessToken, string RefreshToken);

public class SignInHandler(
    IUserRepo userRepo,
    ITokenService tokenService, 
    IPasswordService passwordService) 
    : IRRequestHandler<SignIn, Tokens>
{
    public async Task<Result<Tokens>> Handle(SignIn request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetByUsernameAsync(request.UsernameOrEmail) 
                   ?? await userRepo.GetByEmailAsync(request.UsernameOrEmail);
        
        if (user == null)
            return Failure.Validation("Invalid Username Or Password");

        if (passwordService.VerifyHashedPassword(user, request.Password))
            return new Tokens(
                tokenService.GenerateAccessToken(user),
                tokenService.GenerateRefreshToken(user)
            );
        
        return Failure.Validation("Invalid Username/Email Or Password");
    }
}
