using Microsoft.EntityFrameworkCore;

namespace EfCorePerformance.Data;

public static class ServiceCollectionExtenders
{
    public static IServiceCollection ConfigureDb(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("companies");

        services.AddDbContext<CompanyDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}