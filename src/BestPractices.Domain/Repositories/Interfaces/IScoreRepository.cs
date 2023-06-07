using BestPractices.Domain.Entities;

namespace BestPractices.Domain.Repositories.Interfaces;

public interface IScoreRepository
{
    public Task SaveAsync(Score score);
    public Task<Score?> GetAsync(string id);
}
