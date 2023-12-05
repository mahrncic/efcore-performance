using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EfCorePerformance.Data;

public sealed class CompanyDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; } = default!;
    public DbSet<Employee> Employees { get; set; } = null!;
    
    public CompanyDbContext()
    {
    }

    public CompanyDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}