using Microsoft.Extensions.DependencyInjection;
using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Nutrition;
using NutriFitLogBackend.Infrastructure.Repositories.Trainings;
using NutriFitLogBackend.Infrastructure.Repositories.Users;

namespace NutriFitLogBackend.Infrastructure.Extensions;

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
                new NutritionRepository(context)
            );
        });
        return serviceCollection;
    }
}
