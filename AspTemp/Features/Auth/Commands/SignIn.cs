using AspTemp.Features.Auth.Services;
using AspTemp.Shared.Application.Contracts.Cqrs;
using AspTemp.Shared.Application.Contracts.ResultContracts;

namespace AspTemp.Features.Auth.Commands;

public record SignIn(
    string Username,
    string Password
): IRRequest<string>;

public class SignInHandler(
    IUserRepo userRepo,
    IJwtService jwtService, 
    IPasswordService passwordService) 
    : IRRequestHandler<SignIn, string>
{
    public async Task<Result<string>> Handle(SignIn request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetByUsernameAsync(request.Username);
        if (user == null) 
            return Failure.Validation("Invalid Username Or Password");

        if (passwordService.VerifyHashedPassword(user, request.Password))
            return await jwtService.Generate(user);
        
        return Failure.Validation("Invalid Username Or Password");
    }
}
