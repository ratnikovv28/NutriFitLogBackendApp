using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Trainings;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(200);

        builder.Property(e => e.PictureUrl)
            .HasMaxLength(2048);

        builder.Property(e => e.CreatedDate)
            .IsRequired();

        builder.Property(e => e.Type)
            .HasConversion<int>();

        builder.HasMany(e => e.Trainings)
            .WithOne(te => te.Exercise)
            .HasForeignKey(te => te.ExerciseId);
    }
}