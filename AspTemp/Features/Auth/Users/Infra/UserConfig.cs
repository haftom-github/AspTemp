using AspTemp.Features.Auth.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspTemp.Features.Auth.Users.Infra;

public class UserConfig: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Email)
            .HasMaxLength(100);

        builder.HasMany(x => x.AuthIdentities)
            .WithOne()
            .HasForeignKey(y => y.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Navigation(x => x.AuthIdentities)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(x => x.LocalAuthIdentity);
    }
}