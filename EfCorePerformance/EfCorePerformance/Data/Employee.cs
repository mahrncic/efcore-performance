namespace EfCorePerformance.Data;

public sealed class Employee
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Age { get; set; }
    public string Role { get; set; } = default;
    
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;
}