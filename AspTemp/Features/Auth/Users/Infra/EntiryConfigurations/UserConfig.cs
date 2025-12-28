using AspTemp.Features.Auth.AuthProviders.Domain;
using AspTemp.Features.Auth.Users.Domain;
using AspTemp.Features.Auth.Users.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspTemp.Features.Auth.Users.Infra.EntiryConfigurations;

public class UserConfig(IPasswordService passwordService): IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasMany(x => x.AuthIdentities)
            .WithOne()
            .HasForeignKey(y => y.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.LocalAuthIdentity);
        
        var now = new DateTime(2025, 12, 25);
        var admin = new User
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Email = "ftomtse@gmail.com",
            CreatedDate = now
        };
        
        admin.AddAuthIdentity(AuthProvider.LocalAuthProviderId, "admin", passwordService.Hash("admin"));
        builder.HasData(admin);
    }
}