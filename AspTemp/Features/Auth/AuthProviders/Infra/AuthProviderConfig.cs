using AspTemp.Features.Auth.AuthProviders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspTemp.Features.Auth.AuthProviders.Infra;

public class AuthProviderConfig: IEntityTypeConfiguration<AuthProvider>
{
    public void Configure(EntityTypeBuilder<AuthProvider> builder)
    {
        builder.ToTable("authProviders");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasMaxLength(20);

        builder.Property(x => x.ClientId)
            .HasMaxLength(100);
    }
}