using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
           var db = scope.ServiceProvider.GetRequiredService<NutriFitLogContext>();
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}