namespace BestPractices.Domain.Entities;

public class Loan
{
    public string Id { get; set; }
    public decimal Value { get; set; }
    public Customer Borrower { get; set; }
    public DateTime RepaymentDate { get; set; }
    public double FeesByMonth { get; set; }

    public Loan()
    {
        Id = new Guid().ToString();
    }
}
