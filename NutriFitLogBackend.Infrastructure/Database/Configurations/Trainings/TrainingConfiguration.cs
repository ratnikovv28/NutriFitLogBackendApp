using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Trainings;

public class TrainingConfiguration : IEntityTypeConfiguration<Domain.Entities.Trainings.Training>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Trainings.Training> builder)
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