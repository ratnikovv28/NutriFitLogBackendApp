using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Nutrition;

public class DayPartConfiguration : IEntityTypeConfiguration<DayPart>
{
    public void Configure(EntityTypeBuilder<DayPart> builder)
    {
        builder.HasKey(dp => dp.Id);

        builder.Property(dp => dp.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasMany(dp => dp.Meals)
            .WithOne(mf => mf.DayPart)
            .HasForeignKey(mf => mf.DayPartId);
    }
}
