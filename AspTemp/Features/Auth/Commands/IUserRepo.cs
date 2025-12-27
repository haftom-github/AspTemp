using AspTemp.Features.Auth.Domain;
using AspTemp.Features.Auth.Services;

namespace AspTemp.Features.Auth.Commands;

public interface IUserRepo
{
    Task AddAsync(User user);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
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

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task<bool> UsernameExistsAsync(string username)
    {
        return Task.FromResult(_users.Any(u => u.Username == username));
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        return Task.FromResult(_users.Any(u => u.Email == email));
    }

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