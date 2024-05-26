using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Infrastructure.Database.Configurations.Users;

public class StudentTrainerConfiguration : IEntityTypeConfiguration<StudentTrainer>
{
    public void Configure(EntityTypeBuilder<StudentTrainer> builder)
    {
        builder.HasKey(st => st.Id);

        builder.HasOne(st => st.Student)
            .WithMany(u => u.Trainers)
            .HasForeignKey(st => st.StudentId);

        builder.HasOne(st => st.Trainer)
            .WithMany(u => u.Students)
            .HasForeignKey(st => st.TrainerId);
        
        builder.HasIndex(st => new { st.TrainerId, st.StudentId }).IsUnique();
    }
}