using BestPractices.Domain.Entities;

namespace BestPractices.Domain.Repositories.Interfaces;

public interface ILoanRepository
{
    public Task SaveAsync(Loan loan);
}
