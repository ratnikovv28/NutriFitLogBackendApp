using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Trainings;

public class TrainingConfiguration : IEntityTypeConfiguration<Training>
{
    public void Configure(EntityTypeBuilder<Training> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.CreatedDate)
            .IsRequired();

        builder.HasOne(t => t.User)
            .WithMany(u => u.Trainings)
            .HasForeignKey(t => t.UserId);

        builder.HasMany(t => t.Exercises)
            .WithOne(te => te.Training)
            .HasForeignKey(te => te.TrainingId);
    }
}