using AspTemp.Features.Auth.Services;
using AspTemp.Shared.Application.Contracts.Cqrs;
using AspTemp.Shared.Application.Contracts.ResultContracts;

namespace AspTemp.Features.Auth.Commands;

public record SignIn(
    string UsernameOrEmail,
    string Password
): IRRequest<Tokens>;

public class SignInHandler(
    IUserRepo userRepo,
    ITokenService tokenService, 
    IPasswordService passwordService) 
    : IRRequestHandler<SignIn, Tokens>
{
    public async Task<Result<Tokens>> Handle(SignIn request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetByUsernameAsync(request.UsernameOrEmail, cancellationToken) 
                   ?? await userRepo.GetByEmailAsync(request.UsernameOrEmail, cancellationToken);
        
        if (user == null)
            return Failure.Validation("Invalid Username Or Password");

        var userPassword = user.LocalAuthIdentity!.Password!;
        if (passwordService.Verify(user, userPassword, request.Password))
            return tokenService.GenerateTokens(user);
        
        return Failure.Validation("Invalid Username/Email Or Password");
    }
}
