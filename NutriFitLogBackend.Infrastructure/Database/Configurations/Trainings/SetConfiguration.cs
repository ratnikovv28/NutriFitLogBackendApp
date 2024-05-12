using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Trainings;

public class SetConfiguration : IEntityTypeConfiguration<Set>
{
    public void Configure(EntityTypeBuilder<Set> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Repetitions)
            .IsRequired(false);

        builder.Property(s => s.Weight)
            .HasColumnType("double precision")
            .IsRequired(false);

        builder.Property(s => s.Duration)
            .HasColumnType("double precision")
            .IsRequired(false);

        builder.Property(s => s.Distance)
            .HasColumnType("double precision")
            .IsRequired(false);

        builder.HasOne(s => s.TrainingExercise)
            .WithMany(te => te.Sets)
            .HasForeignKey(s => new { s.TrainingId, s.ExerciseId });
    }
}
