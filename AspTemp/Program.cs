using AspTemp.Features.Auth;
using AspTemp.Features.Auth.Commands;
using AspTemp.Features.Auth.Services;
using AspTemp.Shared.Application;
using AspTemp.Shared.Application.Contracts.ResultContracts;
using AspTemp.Shared.Application.Contracts.ResultContracts.Extensions;
using MediatR;

var assembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.ConfigureApplication(assembly);
builder.Services.ConfigureAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapPost("/api/auth/signin", async (
        SignIn request, 
        ISender sender, 
        ITokenService tokenService,
        HttpContext httpContext,
        CancellationToken ct) =>
    {
        var result = await sender.Send(request, ct);
        if (!result.IsSuccess) return result.ToHttpResult();

        httpContext.Response.Cookies.Append(
            "access_token",
            result.Success!.Value.AccessToken,
            tokenService.AccessTokenCookieOptions
        );

        httpContext.Response.Cookies.Append(
            "refresh_token",
            result.Success!.Value.RefreshToken,
            tokenService.RefreshTokenCookieOptions
        );
        
        return Result.Ok().ToHttpResult();
    })
    .WithName("auth");

app.MapGet("/api/auth/refresh",
    async (ISender sender, ITokenService tokenService, HttpContext httpContext) =>
    {
        var refreshToken = httpContext.Request.Cookies["refresh_token"];
        if (refreshToken == null)
            return Result.Failure(Failure.Unauthorized())
                .ToHttpResult();
        
        var result = await sender.Send(new Refresh(refreshToken), CancellationToken.None);
        if (!result.IsSuccess) return result.ToHttpResult();

        httpContext.Response.Cookies.Append(
            "access_token",
            result.Success!.Value.accessToken,
            tokenService.AccessTokenCookieOptions
        );
        
        httpContext.Response.Cookies.Append(
            "refresh_token",
            result.Success!.Value.refreshToken,
            tokenService.RefreshTokenCookieOptions
        );
        
        return Result.Ok().ToHttpResult();
    });

app.MapGet("/api/home", () => "Hello World!")
    .WithName("home")
    .RequireAuthorization();

app.Run();

