using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Nutrition;

public class MealFoodConfiguration : IEntityTypeConfiguration<MealFood>
{
    public void Configure(EntityTypeBuilder<MealFood> builder)
    {
        builder.HasKey(mf => mf.Id);

        builder.Property(mf => mf.PortionDescription)
            .HasMaxLength(200); 

        builder.Property(mf => mf.Calories).HasColumnType("double precision").IsRequired(false);
        builder.Property(mf => mf.Protein).HasColumnType("double precision").IsRequired(false);
        builder.Property(mf => mf.Fats).HasColumnType("double precision").IsRequired(false);
        builder.Property(mf => mf.Carbohydrates).HasColumnType("double precision").IsRequired(false);

        builder.Property(mf => mf.Unit).HasConversion<int>();
        builder.Property(mf => mf.Quantity).HasColumnType("double precision");

        builder.HasOne(mf => mf.DayPart)
            .WithMany(dp => dp.Meals)
            .HasForeignKey(mf => mf.DayPartId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(mf => mf.Meal)
            .WithMany(m => m.Foods)
            .HasForeignKey(mf => mf.MealId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(mf => mf.Food)
            .WithMany(f => f.Meals)
            .HasForeignKey(mf => mf.FoodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}