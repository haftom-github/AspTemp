using AspTemp.Features.Auth.Services;

namespace AspTemp.Features.Auth.Commands;

public interface IUserRepo
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
}

public class UserRepo(IPasswordService passwordService): IUserRepo
{
    private readonly List<User> _users = [
        new()
        {
            Email = "ftomtse@gmail.com",
            Password = passwordService.Hash("pass"),
            Username = "john"
        }
    ];

    public Task<User?> GetByUsernameAsync(string username)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Username == username));
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
    }
}