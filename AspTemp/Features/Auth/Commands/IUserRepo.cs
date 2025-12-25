using AspTemp.Features.Auth.Services;

namespace AspTemp.Features.Auth.Commands;

public interface IUserRepo
{
    Task<User?> GetByUsernameAsync(string username);
}

public class UserRepo(IPasswordService passwordService): IUserRepo
{
    private readonly List<User> _users = [
        new()
        {
            Email = "john@gmail.com",
            Firstname = "John",
            MiddleName = "Kyle",
            Lastname = "Doe",
            Password = passwordService.HashPassword("pass"),
            Username = "john"
        }
    ];

    public Task<User?> GetByUsernameAsync(string username)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Username == username));
    }
}