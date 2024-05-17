using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Nutrition;

public class MealFoodConfiguration : IEntityTypeConfiguration<MealFood>
{
    public void Configure(EntityTypeBuilder<MealFood> builder)
    {
        builder.HasKey(mf => mf.Id);

        builder.Property(mf => mf.Calories).HasColumnType("double precision").IsRequired(false);
        builder.Property(mf => mf.Protein).HasColumnType("double precision").IsRequired(false);
        builder.Property(mf => mf.Fats).HasColumnType("double precision").IsRequired(false);
        builder.Property(mf => mf.Carbohydrates).HasColumnType("double precision").IsRequired(false);
        
        builder.Property(mf => mf.Quantity).HasColumnType("double precision");

        builder.HasOne(mf => mf.DayPart)
            .WithMany(dp => dp.Meals)
            .HasForeignKey(mf => mf.DayPartId);

        builder.HasOne(mf => mf.Meal)
            .WithMany(m => m.Foods)
            .HasForeignKey(mf => mf.MealId);

        builder.HasOne(mf => mf.Food)
            .WithMany(f => f.Meals)
            .HasForeignKey(mf => mf.FoodId);
    }
}