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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/auth/signin", async (SignIn request, ISender sender, CancellationToken ct) =>
    {
        var result = await sender.Send(request, ct);
        return result.ToHttpResult();
    })
    .WithName("GetWeatherForecast");

app.MapGet("/paginated", async ([AsParameters] GetAllUsers request, ISender sender, CancellationToken ct) =>
{
    var result = await sender.Send(request, ct);
    return result.ToHttpResult();
});

app.Run();

