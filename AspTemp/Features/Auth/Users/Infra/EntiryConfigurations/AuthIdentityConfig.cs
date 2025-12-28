using AspTemp.Features.Auth.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspTemp.Features.Auth.Users.Infra.EntiryConfigurations;

public class AuthIdentityConfig: IEntityTypeConfiguration<AuthIdentity>
{
    public void Configure(EntityTypeBuilder<AuthIdentity> builder)
    {
        builder.ToTable("authIdentities");
        builder.HasKey(x => new { x.UserId, x.AuthProviderId });
        
        builder.HasOne(x => x.AuthProvider)
            .WithMany()
            .HasForeignKey(x => x.AuthProviderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.ProviderUserId)
            .HasMaxLength(128);

        builder.Property(x => x.Password)
            .HasMaxLength(256);
    }
}