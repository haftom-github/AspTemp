using AspTemp.Features.Auth.Commands;
using AspTemp.Features.Auth.Services;
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
    
    [HttpPost("signin")]
    public async Task<IResult> SignInAsync(SignIn request, CancellationToken ct)
    {
        var result = await sender.Send(request, ct);
        if (!result.IsSuccess) return result.ToHttpResult();

        AttachTokensToCookie(Response, result.Success!.Value);
        
        return Result.Ok().ToHttpResult();
    }

    [HttpPost("signin/google")]
    public async Task<IResult> SignInWithGoogle(SignInWithGoogle request, CancellationToken ct)
    {
        var result = await sender.Send(request, ct);
        if (!result.IsSuccess) return result.ToHttpResult();
        
        AttachTokensToCookie(Response, result.Success!.Value);
        
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

        AttachTokensToCookie(Response, result.Success!.Value);
        
        return Result.Ok().ToHttpResult();
    }

    private void AttachTokensToCookie(HttpResponse response, Tokens tokens)
    {
        var accessTokenCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            // todo: make it secure at production
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(int.Parse(config["Jwt:AccessTokenMinutes"]!))
        };
        
        var refreshTokenCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            // todo: make it secure at production
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(int.Parse(config["Jwt:RefreshTokenMinutes"]!)),
            Path = "/api/auth/refresh"
        };
        
        response.Cookies.Append(
            _accessTokenSessionKey,
            tokens.AccessToken,
            accessTokenCookieOptions
        );
        
        response.Cookies.Append( 
            _refreshTokenSessionKey,
            tokens.RefreshToken,
            refreshTokenCookieOptions
        );
    }
}