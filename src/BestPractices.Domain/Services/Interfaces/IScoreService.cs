using BestPractices.Domain.ValueObjects;

namespace BestPractices.Domain.Services.Interfaces;

public interface IScoreService
{
    public Task CreateScore(string customerId);
}
