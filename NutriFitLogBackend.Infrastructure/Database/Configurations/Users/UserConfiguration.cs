using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.TelegramId)
            .IsUnique();

        builder.HasMany(u => u.Students)
            .WithMany(u => u.Trainers);
        
        builder.HasMany(u => u.Trainers)
            .WithMany(u => u.Students);

        builder.HasMany(u => u.Actions)
            .WithOne(a => a.Admin)
            .HasForeignKey(a => a.AdminId);

        builder.HasMany(u => u.Trainings)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);
        
        builder.HasMany(u => u.Meals)
            .WithOne(m => m.User)
            .HasForeignKey(m => m.UserId);
    }
}