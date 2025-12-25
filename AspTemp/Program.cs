using AspTemp.Features.Auth;
using AspTemp.Features.Auth.Commands;
using AspTemp.Shared.Application;
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
        HttpContext httpContext,
        CancellationToken ct) =>
    {
        var result = await sender.Send(request, ct);
        if (!result.IsSuccess) return result.ToHttpResult();
        
        httpContext.Response.Cookies.Append(
            "access_token", 
            result.Success!.Value,
            new CookieOptions
            {
                HttpOnly = true,
                // todo: make it secure at production
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(24)
            });
        return Results.Ok();
    })
    .WithName("auth");

app.MapGet("/api/home", () => "Hello World!")
    .WithName("home")
    .RequireAuthorization();

app.Run();

