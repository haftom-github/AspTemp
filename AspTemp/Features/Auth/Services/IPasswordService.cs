using Microsoft.AspNetCore.Identity;

namespace AspTemp.Features.Auth.Services;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyHashedPassword(User user, string providedPassword);
}

public class PasswordService(IPasswordHasher<User> passwordHasher): IPasswordService
{
    public string HashPassword(string password)
        => passwordHasher.HashPassword(new User(), password);

    public bool VerifyHashedPassword(User user, string providedPassword)
        => passwordHasher.VerifyHashedPassword(user, user.Password, providedPassword) == PasswordVerificationResult.Success;
}
