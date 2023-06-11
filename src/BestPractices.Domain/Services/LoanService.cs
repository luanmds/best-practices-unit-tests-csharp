using BestPractices.Domain.Entities;
using BestPractices.Domain.Repositories.Interfaces;
using BestPractices.Domain.Services.Interfaces;

namespace BestPractices.Domain.Services;

public class LoanService : ILoanService
{
    private const decimal MaxLoanValue = 10000.0M;

    private ILoanRepository _repository;
    private IScoreRepository _scoreRepository;

    public LoanService(ILoanRepository repository, IScoreRepository scoreRepository)
    {
        _repository = repository;
        _scoreRepository = scoreRepository;
    }

    public async Task CreateLoan(string scoreId, DateTime repaymentDate)
    {
        var score = await _scoreRepository.GetAsync(scoreId);
        
        if (score == null)
            throw new ArgumentNullException("Score is not found!");

        var loanValue = CalculateLoanValue(score.Customer.DebitsAmount, score.ScoreValue);
        var feesByMonth = CalculateFeesByMonth(score.ScoreValue);

        var loan = new Loan()
        {
            Value = loanValue,
            Borrower = score.Customer,
            RepaymentDate = repaymentDate,
            FeesByMonth = feesByMonth,
        };

        await _repository.SaveAsync(loan);
    }

    public decimal CalculateLoanValue(decimal debitsAmount, int score)
    {        
        if (score == 0) return 0;

        decimal LoanValue = (score * 100) - debitsAmount;
        
        return LoanValue > MaxLoanValue ? MaxLoanValue : LoanValue;
    }

    public double CalculateFeesByMonth(int score)
    {
        return score switch
        {
            >= 90 and <= 100 => 12.5,
            < 90 and >= 50 => 16,
            _ => 19.3,
        };
    }
}
