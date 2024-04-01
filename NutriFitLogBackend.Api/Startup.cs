using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Extensions;
using NutriFitLogBackend.Middlewares;

namespace NutriFitLogBackend;

public class Startup
{   
    public IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblies(Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load));
        
        //TODO Change to PostgreSQL
        services.AddDbContext<NutriFitLogContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("NutriFitLogContext")));

        services.SetupUnitOfWork();

        services.AddAutoMapper(Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load));
        
        //TODO Change repositories
        // services.AddScoped<IPermissionService, PermissionService>();
        // services.AddScoped<IPermissionTypeService, PermissionTypeService>();

        var client = Configuration.GetSection("ClientHost").Value;

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .WithOrigins(Configuration.GetSection("ClientHost").Value)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );
        });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "NutriFitLogBackend.API", Version = "v1" });
        });
    }
    
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NutriFitLogBackend.API v1"));
        }

        app.UseHttpsRedirection();

        app.UseCors("CorsPolicy");

        // Use Exception Middleware
        app.UseMiddleware<ExceptionHandler>();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}