using AspTemp.Features.Auth.Commands;
using AspTemp.Shared.Application.Contracts.ResultContracts;
using AspTemp.Shared.Application.Contracts.ResultContracts.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspTemp.Features.Auth;

[ApiController]
[Route("/api/auth/")]
public class AuthController(ISender sender, IConfiguration config): ControllerBase
{
    private readonly string _refreshTokenSessionKey = config["Jwt:RefreshTokenSessionKey"]!;
    private readonly string _accessTokenSessionKey = config["Jwt:AccessTokenSessionKey"]!;

    private readonly CookieOptions _accessTokenCookieOptions = new()
    {
        HttpOnly = true,
        // todo: make it secure at production
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddMinutes(int.Parse(config["Jwt:AccessTokenMinutes"]!))
    };
    
    private readonly CookieOptions _refreshTokenCookieOptions = new()
    {
        HttpOnly = true,
        // todo: make it secure at production
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddMinutes(int.Parse(config["Jwt:RefreshTokenMinutes"]!)),
        Path = "/api/auth/refresh"
    };
    
    [HttpPost("signin")]
    public async Task<IResult> SignInAsync(SignIn request, CancellationToken ct)
    {
        var result = await sender.Send(request, ct);
        if (!result.IsSuccess) return result.ToHttpResult();

        Response.Cookies.Append(
            _accessTokenSessionKey,
            result.Success!.Value.AccessToken,
            _accessTokenCookieOptions
        );

        Response.Cookies.Append( 
            _refreshTokenSessionKey,
            result.Success!.Value.RefreshToken,
            _refreshTokenCookieOptions
        );
        
        return Result.Ok().ToHttpResult();
    }

    [HttpPost("signin/google")]
    public async Task<IResult> SignInWithGoogle(SignInWithGoogle request, CancellationToken ct)
    {
        var result = await sender.Send(request, ct);
        if (!result.IsSuccess) return result.ToHttpResult();
        
        Response.Cookies.Append(
            _accessTokenSessionKey,
            result.Success!.Value.AccessToken,
            _accessTokenCookieOptions
        );

        Response.Cookies.Append( 
            _refreshTokenSessionKey,
            result.Success!.Value.RefreshToken,
            _refreshTokenCookieOptions
        );
        
        return Result.Ok().ToHttpResult();
    }

    [HttpGet("refresh")]
    public async Task<IResult> Refresh(CancellationToken ct)
    {
        var refreshToken = Request.Cookies[_refreshTokenSessionKey];
        if (refreshToken == null)
            return Result.Failure(Failure.Unauthorized())
                .ToHttpResult();
        
        var result = await sender.Send(new Refresh(refreshToken), ct);
        if (!result.IsSuccess) return result.ToHttpResult();

        Response.Cookies.Append(
            _accessTokenSessionKey,
            result.Success!.Value.accessToken,
            _accessTokenCookieOptions
        );
        
        Response.Cookies.Append( 
            _refreshTokenSessionKey,
            result.Success!.Value.refreshToken,
            _refreshTokenCookieOptions
        );
        
        return Result.Ok().ToHttpResult();
    }
}