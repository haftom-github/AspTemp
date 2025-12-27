using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AspTemp.Features.Auth.Commands;
using AspTemp.Features.Auth.Domain;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AspTemp.Features.Auth.Services;

public interface ITokenService
{
    Tokens GenerateTokens(User user);
    Task<Tokens?> Refresh(string refreshToken);
}

public record Tokens(string AccessToken, string RefreshToken);

public class TokenService(IConfiguration config, IUserRepo userRepo): ITokenService
{
    private string GenerateAccessToken(User user)
    {
        var jwt = config.GetSection("Jwt");
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["AccessTokenMinutes"]!)),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken(User user)
    {
        var refreshTokenMinutes = int.Parse(config["Jwt:RefreshTokenMinutes"]!);
        var bytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(bytes);
        
        RefreshTokenStore.Add(refreshToken, user.Id, TimeSpan.FromMinutes(refreshTokenMinutes));
        return refreshToken;
    }

    public Tokens GenerateTokens(User user) => new(GenerateAccessToken(user), GenerateRefreshToken(user));

    public async Task<Tokens?> Refresh(string refreshToken)
    {
        RefreshTokenStore.TryConsume(refreshToken, out var userId);
        if (userId == null)
            return null;
        
        var user = await userRepo.GetByIdAsync(userId.Value);

        return user == null 
            ? null : new Tokens(GenerateAccessToken(user), GenerateRefreshToken(user));
    }
}