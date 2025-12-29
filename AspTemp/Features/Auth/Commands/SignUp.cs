using AspTemp.Features.Auth.Domain;
using AspTemp.Features.Auth.Services;
using AspTemp.Shared.Application.Contracts;
using AspTemp.Shared.Application.Contracts.Cqrs;
using AspTemp.Shared.Application.Contracts.ResultContracts;
using AspTemp.Shared.Domain;
using FluentValidation;

namespace AspTemp.Features.Auth.Commands;

public record SignUp(
    string Username,
    string? Email,
    string Password
) : ICleanable<SignUp>, IRRequest
{
    public SignUp Clean
        => this with
        {
            Username = Username.Trim().ToLowerInvariant(),
            Email = Email?.Trim().ToLowerInvariant()
        };
}

public class SignUpHandler(IUserRepo userRepo, IAuthProviderRepo authProviderRepo, IPasswordService passwordService): IRRequestHandler<SignUp>
{
    public async Task<Result<Unit>> Handle(SignUp request, CancellationToken cancellationToken)
    {
        if (await userRepo.UsernameExistsAsync(request.Username, cancellationToken))
            return Failure.Validation(nameof(request.Username), "Username already exists");

        if (!string.IsNullOrWhiteSpace(request.Email) && await userRepo.EmailExistsAsync(request.Email, cancellationToken))
            return Failure.Validation(nameof(request.Email), "Email already exists");

        var user = new User
        {
            Email = request.Email,
        };

        var localAuthProvider = await authProviderRepo.GetByNameAsync("local");

        if (localAuthProvider == null)
            return Failure.NotFound("Local authentication not available");
        
        user.AddAuthIdentity(localAuthProvider.Id, request.Username, passwordService.Hash(request.Password));
        
        await userRepo.AddAsync(user, cancellationToken);
        return new Unit();
    }
}

public class SignUpValidator : AbstractValidator<SignUp>
{
    public SignUpValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username must be at most 50 characters.");

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .MaximumLength(320).WithMessage("Email must be at most 320 characters.");
        });

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("(?=.*[a-z])").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("(?=.*[A-Z])").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"(?=.*\d)").WithMessage("Password must contain at least one digit.");
    }
}