using BestPractices.Domain.DTOs;
using BestPractices.Domain.Entities;
using BestPractices.Domain.Repositories.Interfaces;
using BestPractices.Domain.Services.Interfaces;
using BestPractices.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace BestPractices.Domain.Services;

public class CustomerService : ICustomerService
{
    private ICustomerRepository _repository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ICustomerRepository repository, ILogger<CustomerService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Create(CustomerDto customerDto)
    {
        var customer = customerDto.ToCustomer();
        
        await _repository.SaveAsync(customer);

        _logger.LogInformation("Customer save successfully");
    }

    public async Task<Customer> GetById(string id)
    {
        var customer = await _repository.GetAsync(id);
        
        if (customer == null)
            throw new ArgumentNullException("Customer is not found!");

        return customer;         
    }

    public async Task UpdateStatusScore(ProcessStatus status, string customerId)
    {
        var customer = await _repository.GetAsync(customerId);

        if (customer == null)
            throw new ArgumentNullException("Customer is not found!");

        customer.Status = status;
        string id = await _repository.UpdateAsync(customer);

        _logger.LogInformation($"Customer with ID: {id} update successfully");
    }
}
