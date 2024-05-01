using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Infrastructure.Database.Configurations.Nutrition;
using NutriFitLogBackend.Infrastructure.Database.Configurations.Trainings;
using NutriFitLogBackend.Infrastructure.Database.Configurations.Users;
using Action = NutriFitLogBackend.Domain.Entities.Users.Action;

namespace NutriFitLogBackend.Infrastructure.Database;

public class NutriFitLogContext : DbContext
{
    public NutriFitLogContext(DbContextOptions<NutriFitLogContext> options)
    : base(options)
    {
            
    }
    
    public DbSet<User?> Users { get; set; }
    /*public DbSet<Role> Roles { get; set; }*/
    public DbSet<Action> Actions { get; set; }
        
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Set> Sets { get; set; }
    public DbSet<Training> Trainings { get; set; }
    public DbSet<TrainingExercise> TrainingExercise { get; set; }
    
    public DbSet<DayPart> DayParts { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<MealFood> MealFoods { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        /*modelBuilder.ApplyConfiguration(new RoleConfiguration());*/
        modelBuilder.ApplyConfiguration(new ActionConfiguration());
        
        modelBuilder.ApplyConfiguration(new ExerciseConfiguration());
        modelBuilder.ApplyConfiguration(new SetConfiguration());
        modelBuilder.ApplyConfiguration(new TrainingConfiguration());
        modelBuilder.ApplyConfiguration(new TrainingExerciseConfiguration());
        
        modelBuilder.ApplyConfiguration(new DayPartConfiguration());
        modelBuilder.ApplyConfiguration(new FoodConfiguration());
        modelBuilder.ApplyConfiguration(new MealConfiguration());
        modelBuilder.ApplyConfiguration(new MealFoodConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}