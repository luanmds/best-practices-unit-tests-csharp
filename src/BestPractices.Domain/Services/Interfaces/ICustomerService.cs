using BestPractices.Domain.DTOs;
using BestPractices.Domain.Entities;
using BestPractices.Domain.ValueObjects;

namespace BestPractices.Domain.Services.Interfaces;

public interface ICustomerService
{
    public Task<Customer> GetById(string id);
    public Task Create(CustomerDto customerDto);
    public Task UpdateStatusScore(ProcessStatus status, string customerId);
}
