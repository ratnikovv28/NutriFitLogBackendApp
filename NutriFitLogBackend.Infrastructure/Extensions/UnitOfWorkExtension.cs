using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Nutrition;
using NutriFitLogBackend.Infrastructure.Repositories.Trainings;
using NutriFitLogBackend.Infrastructure.Repositories.Users;

namespace NutriFitLogBackend.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class UnitOfWorkExtension
{
    public static IServiceCollection SetupUnitOfWork(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>(f =>
        {
            var context = f.GetService<NutriFitLogContext>();
            return new UnitOfWork(
                context,
                new UserRepository(context),
                new TrainingRepository(context),
                new NutritionRepository(context),
                new ExercisesRepository(context),
                new SetRepository(context),
                new FoodRepository(context),
                new DayPartRepository(context),
                new TrainingExerciseRepository(context),
                new MealFoodRepository(context),
                new StudentTrainerRepository(context)
            );
        });
        return serviceCollection;
    }
}
