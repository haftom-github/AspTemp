using AspTemp.Features.Auth.Domain;
using Microsoft.AspNetCore.Identity;

namespace AspTemp.Features.Auth.Services;

public interface IPasswordService
{
    string Hash(string password);
    bool Verify(User user, string providedPassword);
}

public class PasswordService(IPasswordHasher<User> passwordHasher): IPasswordService
{
    public string Hash(string password)
        => passwordHasher.HashPassword(new User(), password);

    public bool Verify(User user, string providedPassword)
        => passwordHasher.VerifyHashedPassword(user, user.Password, providedPassword) == PasswordVerificationResult.Success;
}
