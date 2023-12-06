using BenchmarkDotNet.Attributes;
using EfCorePerformance.Console.Data;
using Microsoft.EntityFrameworkCore;

namespace EfCorePerformance.Console;

[MemoryDiagnoser]
public class CompiledQueries
{
    private static readonly Func<CompanyDbContext, IAsyncEnumerable<Company>> _compiledQuery
        = EF.CompileAsyncQuery((CompanyDbContext context) => context.Companies.Where(c => c.Name.Length > 10));

    private CompanyDbContext _context;
    private DbContextOptions<CompanyDbContext> _options;
    
    [GlobalSetup]
    public void Setup()
    {
        var connString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=companies";
        _options = new DbContextOptionsBuilder<CompanyDbContext>()
            .UseNpgsql(connString)
            .Options;

        _context = new CompanyDbContext(_options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _context.SeedData();
    }

    [Benchmark]
    public async ValueTask<int> WithCompiledQuery()
    {
        var idSum = 0;

        await foreach (var company in _compiledQuery(_context))
        {
            idSum += company.Id;
        }

        return idSum;
    }

    [Benchmark]
    public async ValueTask<int> WithoutCompiledQuery()
    {
        var idSum = 0;

        await foreach (var blog in _context.Companies.Where(c => c.Name.Length > 10).AsAsyncEnumerable())
        {
            idSum += blog.Id;
        }

        return idSum;
    }

    [GlobalCleanup]
    public ValueTask Cleanup() => _context.DisposeAsync();
}