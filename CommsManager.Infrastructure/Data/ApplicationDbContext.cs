using Microsoft.EntityFrameworkCore;
using CommsManager.Core.Entities;
using System.Reflection;
using System.Text.Json;

namespace CommsManager.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<ArtistProfile> ArtistProfiles => Set<ArtistProfile>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.GetTableName() == entityType.DisplayName())
            {
                entityType.SetTableName(entityType.DisplayName() + "s");
            }

            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                {
                    property.SetMaxLength(200);
                }
                if (property.ClrType.IsEnum)
                {
                    property.SetColumnType("nvarchar(50)");
                }
            }
        }

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.SetConstraintName($"FK_{relationship.DeclaringEntityType.GetTableName()}_{relationship.PrincipalEntityType.GetTableName()}");
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>()
            .HaveColumnType("datetime2")
            .HavePrecision(0);

        configurationBuilder.Properties<Guid>()
            .HaveColumnType("uniqueidentifier");

        configurationBuilder.Properties<decimal>()
            .HavePrecision(18, 2);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added ||
                e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                //TODO: логика для даты создания
            }

            //TODO: логика для даты обновления
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}