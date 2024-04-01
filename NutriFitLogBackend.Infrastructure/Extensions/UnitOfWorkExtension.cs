using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using NutriFitLogBackend.Domain.Repositories;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure.Extensions;

public static class UnitOfWorkExtension
{
    public static IServiceCollection SetupUnitOfWork([NotNull] this IServiceCollection serviceCollection)
    {
        //TODO: Find a way to inject the repositories and share the same context without creating a instance.
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>(f =>
        {
            var scopeFactory = f.GetRequiredService<IServiceScopeFactory>();
            var context = f.GetService<NutriFitLogContext>();
            return new UnitOfWork(
                context/*,
                new PermissionRepository(context.Permissions),
                new PermissionTypeRepository(context.PermissionTypes)*/
            );
        });
        return serviceCollection;
    }
}
