using BestPractices.Domain.Entities;

namespace BestPractices.Domain.Repositories.Interfaces;

public interface ICustomerRepository
{
    public Task SaveAsync(Customer customer);
    public Task<Customer?> GetAsync(string id);
    public Task<string> UpdateAsync(Customer customer);
}
