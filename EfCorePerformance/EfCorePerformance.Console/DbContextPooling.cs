﻿using BenchmarkDotNet.Attributes;
using EfCorePerformance.Console.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EfCorePerformance.Console;

[MemoryDiagnoser]
public class DbContextPooling
{
    private DbContextOptions<CompanyDbContext> _options;
    private PooledDbContextFactory<CompanyDbContext> _poolingFactory;
    
    [GlobalSetup]
    public void Setup()
    {
        var connString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=companies";
        _options = new DbContextOptionsBuilder<CompanyDbContext>()
            .UseNpgsql(connString)
            .Options;

        using var context = new CompanyDbContext(_options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        context.SeedData();

        _poolingFactory = new PooledDbContextFactory<CompanyDbContext>(_options);
    }
    
    [Benchmark]
    public List<Company> WithoutContextPooling()
    {
        using var context = new CompanyDbContext(_options);

        return context.Companies
            .Take(10)
            .Include(x => x.Employees)
            .ToList();    }

    [Benchmark]
    public List<Company> WithContextPooling()
    {
        using var context = _poolingFactory.CreateDbContext();

        return context.Companies
            .Take(10)
            .Include(x => x.Employees)
            .ToList();
    }
}