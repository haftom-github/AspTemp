using AspTemp.Features.Auth;
using AspTemp.Features.Auth.AuthProviders.Domain;
using AspTemp.Features.Auth.Users.Domain;
using AspTemp.Features.Auth.Users.Services;
using AspTemp.Shared.Application;
using AspTemp.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

var assembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureApplication(assembly);
builder.Services.ConfigureInfrastructure();
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

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    db.Database.Migrate();
    
    var oauthSection = config.GetSection("OAuth");
    foreach (var providerSection in oauthSection.GetChildren())
    {
        var providerName = providerSection.Key.ToLower();
        var clientId = providerSection["ClientId"];

        if (!db.AuthProviders.Any(a => a.Name == providerName))
        {
            db.AuthProviders.Add(new AuthProvider
            {
                Id = Guid.NewGuid(),
                Name = providerName,
                ClientId = clientId
            });
        }
    }
    
    if (!db.AuthProviders.Any(a => a.Name == "local"))
    {
        db.AuthProviders.Add(new AuthProvider
        {
            Id = Guid.NewGuid(),
            Name = "local",
            ClientId = null
        });
    }
    
    db.SaveChanges();
    
    if (!db.Users.Any())
    {
        var localAuth = db.AuthProviders.Single(a => a.Name == "local");
        var admin = new User
        {
            Id = Guid.NewGuid(),
            Email = "ftomtse@gmail.com"
        };

        admin.AddAuthIdentity(
            localAuth.Id,
            "admin",
            passwordService.Hash("admin"));

        db.Users.Add(admin);
        db.SaveChanges();
    }
}

app.Run();
