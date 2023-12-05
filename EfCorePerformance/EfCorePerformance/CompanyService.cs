using BenchmarkDotNet.Attributes;
using EfCorePerformance.Data;
using Microsoft.EntityFrameworkCore;

namespace EfCorePerformance;

[MemoryDiagnoser]
public sealed class CompanyService
{
    private readonly CompanyDbContext _dbContext;
    
    public CompanyService(CompanyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [Benchmark]
    public async Task<List<Company>> GetCompaniesAsync(int numOfCompanies = 100)
    {
        return await _dbContext.Companies
            .Take(numOfCompanies)
            .Include(x => x.Employees)
            .ToListAsync();
    }
}