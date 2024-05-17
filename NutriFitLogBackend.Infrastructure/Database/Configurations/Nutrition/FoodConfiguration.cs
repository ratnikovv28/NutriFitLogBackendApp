using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Nutrition;

public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Description)
            .HasMaxLength(200);
        
        builder.Property(e => e.PictureUrl)
            .HasMaxLength(2048);
        
        builder.Property(mf => mf.Unit).HasConversion<int>();
        
        builder.HasMany(f => f.Meals)
            .WithOne(mf => mf.Food)
            .HasForeignKey(mf => mf.FoodId);
    }
}