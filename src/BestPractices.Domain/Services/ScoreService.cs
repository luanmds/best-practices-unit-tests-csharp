using BestPractices.Domain.Entities;
using BestPractices.Domain.Repositories.Interfaces;
using BestPractices.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BestPractices.Domain.Services;

public class ScoreService : IScoreService
{
    private readonly IScoreRepository _repository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<ScoreService> _logger;

    private const int MaxScoreValue = 100;
    private const int MinScoreValue = 0;

    public ScoreService(IScoreRepository repository, ICustomerRepository customerRepository, ILogger<ScoreService> logger)
    {
        _repository = repository;
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task CreateScore(string customerId)
    {
        var customer = await _customerRepository.GetAsync(customerId);

        if (customer == null)
            throw new ArgumentNullException("Customer is not found!");

        int scoreValue = CalculateScore(customer.DebitsAmount);
        var score = new Score()
        {
            ScoreValue = scoreValue,
            ScoreDueDate = DateTime.UtcNow.AddDays(30),
            Customer = customer
        };

        await _repository.SaveAsync(score);
        _logger.LogInformation($"Score with ID:{score.Id} save successfully");
    }

    public static int CalculateScore(decimal debits)
    {
        // Each 100 in debits, subtractes 10 from score
        if (debits > 1000 || debits < 0) return 0;

        if (debits == 0) return 100;

        int score = MaxScoreValue - (Convert.ToInt32(debits) / 10);

        return score > MaxScoreValue ? MaxScoreValue : score < MinScoreValue ? MinScoreValue : score;
    }
}
