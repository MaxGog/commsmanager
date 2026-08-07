using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommsManager.Core.Interfaces;
using CommsManager.Infrastructure.Data;
using CommsManager.Infrastructure.Repositories;

namespace CommsManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
#if DEBUG
            connectionString = "Server=(localdb)\\mssqllocaldb;Database=CommsManagerDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";
#else
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
#endif
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IArtistProfileRepository, ArtistProfileRepository>();

        return services;
    }
}