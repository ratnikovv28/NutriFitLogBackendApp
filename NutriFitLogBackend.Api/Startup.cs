using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NutriFitLogBackend.Application.Services.Nutrition;
using NutriFitLogBackend.Application.Services.Trainings;
using NutriFitLogBackend.Application.Services.Users;
using NutriFitLogBackend.Domain.Services.Nutrition;
using NutriFitLogBackend.Domain.Services.Trainings;
using NutriFitLogBackend.Domain.Services.Users;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Extensions;
using NutriFitLogBackend.Infrastructure.Mapper;
using NutriFitLogBackend.Middlewares;

namespace NutriFitLogBackend;

[ExcludeFromCodeCoverage]
public class Startup
{   
    public IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        // Добавления валидаторов
        services.AddMvc();
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<Startup>();
        
        // Подключение контекста базы данных
        services.AddDbContext<NutriFitLogContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("NutriFitLogContext")));

        // Подключение паттерна Unit Of Work
        services.SetupUnitOfWork();

        // Добавление автоматического соотношения между бизнес-сущностями и DTO
        services.AddAutoMapper(typeof(MappingProfile));
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITrainingService, TrainingService>();
        services.AddScoped<INutritionService, NutritionService>();

        services.AddCors(options =>
        {
            options.AddPolicy(name: "NutriFitLogPolicy",
                policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod() 
                        .AllowAnyHeader();
                });
        });
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "NutriFitLogBackend.API", Version = "v1" });
        });
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NutriFitLogBackend.API v1"));
        }
        
        app.UseHttpsRedirection();

        app.UseMiddleware<ExceptionHandler>();

        app.UseCors(options =>
            options.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
        );
        
        app.UseRouting();

        app.UseAuthorization();

        app.UseCors("NutriFitLogPolicy");
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}