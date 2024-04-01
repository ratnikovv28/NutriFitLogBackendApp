using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Nutrition;

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.CreatedDate)
            .IsRequired();

        builder.HasOne(m => m.User)
            .WithMany(u => u.Meals)
            .HasForeignKey(m => m.UserId);

        builder.HasMany(m => m.Foods)
            .WithOne(mf => mf.Meal)
            .HasForeignKey(mf => mf.MealId);
    }
}