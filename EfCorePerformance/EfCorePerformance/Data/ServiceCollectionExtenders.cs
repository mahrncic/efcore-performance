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
            // options.UseNpgsql(connectionString, options =>
            // {
            //     options.EnableRetryOnFailure(
            //         maxRetryCount: 5,
            //         maxRetryDelay: TimeSpan.FromSeconds(2),
            //         errorCodesToAdd: new string [] {});
            //
            //     options.MaxBatchSize(1);
            // });

            // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }
}