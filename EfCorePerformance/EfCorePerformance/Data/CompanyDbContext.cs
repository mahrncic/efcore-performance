using System.Reflection;
using Bogus;
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
        SeedData(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }
    
    private void SeedData(ModelBuilder modelBuilder)
    {
        var companyGenerator = new Faker<Company>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.Name, f => f.Company.CompanyName());

        var employeeGenerator = new Faker<Employee>()
            .RuleFor(e => e.Id, f => f.IndexFaker + 1)
            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
            .RuleFor(e => e.LastName, f => f.Name.LastName())
            .RuleFor(e => e.Age, f => f.Random.Int(20, 60))
            .RuleFor(e => e.Role, f => f.Name.JobTitle())
            .RuleFor(e => e.CompanyId, f => f.Random.Int(1, 10_000));
        
        var companies = companyGenerator.Generate(10_000);
        var employees = employeeGenerator.Generate(70_000);

        modelBuilder.Entity<Company>().HasData(companies);
        modelBuilder.Entity<Employee>().HasData(employees);
    }
}