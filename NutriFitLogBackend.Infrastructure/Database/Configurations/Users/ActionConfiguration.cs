using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Action = NutriFitLogBackend.Domain.Entities.Users.Action;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Users;

public class ActionConfiguration : IEntityTypeConfiguration<Action>
{
    public void Configure(EntityTypeBuilder<Action> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.Admin)
            .WithMany(u => u.Actions)
            .HasForeignKey(a => a.AdminId);
    }
}