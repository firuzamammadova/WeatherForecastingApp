using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using WeatherForecastingApp.Application.Common.Interfaces;
using WeatherForecastingApp.Domain.Constants;
using WeatherForecastingApp.Infrastructure;
using WeatherForecastingApp.Infrastructure.Data;
using WeatherForecastingApp.Infrastructure.Identity;
using WeatherForecastingApp.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");


        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();
      //  services.AddTransient<WeatherApiOptions>();


        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IWeatherForecastService, WeatherForeCastService>();

        services.AddHttpClient();
        var weatherApiKey = configuration.GetSection("WeatherApi");

        Guard.Against.Null(weatherApiKey, message: "Api Key 'WeatherApi' not found.");
        services.Configure<WeatherApiOptions>(weatherApiKey);


        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        return services;
    }
}
