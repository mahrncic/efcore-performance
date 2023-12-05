namespace EfCorePerformance.Data;

public sealed class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
}