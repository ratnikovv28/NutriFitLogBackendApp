using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Users;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasMany(r => r.Users)
            .WithMany(u => u.Roles);
    }
}