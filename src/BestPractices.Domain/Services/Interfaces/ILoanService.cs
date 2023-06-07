namespace BestPractices.Domain.Services.Interfaces;

public interface ILoanService
{
    public Task CreateLoan(string scoreId, DateTime repaymentDate);
    public decimal CalculateLoanValue(decimal debitsAmount, int score);
}
