using BestPractices.Domain.ValueObjects;

namespace BestPractices.Domain.Entities;

public class Customer
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public Address Address { get; set; }
    public int Age { get; set; }
    public decimal DebitsAmount { get; set; }
    public ProcessStatus Status { get; set; }

    public Customer()
    {
        Id = new Guid().ToString();
        Status = ProcessStatus.Pending;
    }
        
}
