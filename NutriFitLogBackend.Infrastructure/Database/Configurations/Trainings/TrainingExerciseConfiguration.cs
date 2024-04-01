using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Trainings;

public class TrainingExerciseConfiguration : IEntityTypeConfiguration<TrainingExercise>
{
    public void Configure(EntityTypeBuilder<TrainingExercise> builder)
    {
        builder.HasKey(te => te.Id);

        builder.HasOne(te => te.Training)
               .WithMany(t => t.Exercises)
               .HasForeignKey(te => te.TrainingId);

        builder.HasOne(te => te.Exercise)
               .WithMany(e => e.Trainings)
               .HasForeignKey(te => te.ExerciseId);

        builder.HasMany(te => te.Sets)
               .WithOne(s => s.TrainingExercise)
               .HasForeignKey(s => s.TrainingExerciseId);
    }
}