using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AspTemp.Features.Auth.Commands;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AspTemp.Features.Auth.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    Task<(string accessToken, string refreshToken)?> Refresh(string refreshToken);
    CookieOptions AccessTokenCookieOptions { get; }
    CookieOptions RefreshTokenCookieOptions { get; }
}

public class TokenService(IConfiguration config, IUserRepo userRepo): ITokenService
{
    public string GenerateAccessToken(User user)
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

    public string GenerateRefreshToken(User user)
    {
        var refreshTokenMinutes = int.Parse(config["Jwt:RefreshTokenMinutes"]!);
        var bytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(bytes);
        
        RefreshTokenStore.Add(refreshToken, user.Username, TimeSpan.FromMinutes(refreshTokenMinutes));
        return refreshToken;
    }

    public async Task<(string accessToken, string refreshToken)?> Refresh(string refreshToken)
    {
        RefreshTokenStore.TryConsume(refreshToken, out var username);
        if (username == null)
            return null;
        
        var user = await userRepo.GetByUsernameAsync(username);
        if (user == null)
            return null;
        
        return new ValueTuple<string, string>(GenerateAccessToken(user), GenerateRefreshToken(user));
    }

    public CookieOptions AccessTokenCookieOptions => new()
    {
        HttpOnly = true,
        // todo: make it secure at production
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddMinutes(int.Parse(config["Jwt:AccessTokenMinutes"]!))
    };

    public CookieOptions RefreshTokenCookieOptions => new()
    {
        HttpOnly = true,
        // todo: make it secure at production
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddMinutes(int.Parse(config["Jwt:RefreshTokenMinutes"]!)),
        Path = "/api/auth/refresh"
    };
}