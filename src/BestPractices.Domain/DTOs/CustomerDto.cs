using BestPractices.Domain.Entities;
using BestPractices.Domain.ValueObjects;

namespace BestPractices.Domain.DTOs;

public class CustomerDto
{
    public string Name { get; set; }
    public string Document { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public int? StreetNumber { get; set; }
    public int Age { get; set; }
    public decimal DebitsAmount { get; set; }

    public virtual Customer ToCustomer()
    {
        return new Customer()
        {
            Name = Name,
            Document = Document,
            Age = Age,
            DebitsAmount = DebitsAmount,
            Address = new Address()
            {
              Street = Street,
              City = City,
              StreetNumber = StreetNumber
            }
        };
    }
}
