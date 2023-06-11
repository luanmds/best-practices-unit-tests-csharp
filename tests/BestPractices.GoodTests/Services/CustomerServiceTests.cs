using AutoFixture;
using BestPractices.Domain.DTOs;
using BestPractices.Domain.Entities;
using BestPractices.Domain.Repositories.Interfaces;
using BestPractices.Domain.Services;
using BestPractices.Domain.ValueObjects;
using Castle.Core.Resource;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BestPractices.GoodTests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly Mock<ILogger<CustomerService>> _loggerMock;
        private readonly Fixture _fixture;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
            _loggerMock = new Mock<ILogger<CustomerService>>();
            _fixture = new Fixture();
            _service = new CustomerService(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Save_customer_successfully_when_Dto_is_valid()
        {
            var dto = new Mock<CustomerDto>();

            await _service.Create(dto.Object);

            dto.Verify(x => x.ToCustomer(), Times.Once);
            _repositoryMock.Verify(x => x.SaveAsync(It.IsAny<Customer>()), Times.Once);
            _loggerMock.Verify(l =>
                l.Log(LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Customer save successfully")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public void Save_customer_fails_when_repository_throws_exception()
        {
            var dto = new Mock<CustomerDto>();
            _repositoryMock.Setup(x => x.SaveAsync(It.IsAny<Customer>())).Throws<Exception>();

            Func<Task> result = async () => await _service.Create(dto.Object);

            result.Should().ThrowAsync<Exception>();
            dto.Verify(x => x.ToCustomer(), Times.Once);
        }

        [Fact]
        public async Task GetById_returns_customer_successfully_when_id_is_valid()
        {
            string id = _fixture.Create<string>();
            var customer = _fixture.Build<Customer>().With(x => x.Id, id).Create();
            _repositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(customer);

            var result = await _service.GetById(id);

            result.Should().BeOfType<Customer>();
            _repositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetById_throws_ArgumentNullException_when_id_is_not_found()
        {
            string id = _fixture.Create<string>();

            Func<Task> result = async () => await _service.GetById(id);

            result.Should().ThrowAsync<ArgumentNullException>();
            _repositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData(ProcessStatus.Pending, "123")]
        [InlineData(ProcessStatus.Blocked, "456")]
        [InlineData(ProcessStatus.Processed, "789")]
        public async Task UpdateStatusScore_a_customer_successfully_when_parameters_are_valid(ProcessStatus status, string customerId)
        {
            var customer = _fixture.Build<Customer>().With(x => x.Id, customerId).Create();
            _repositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).
                ReturnsAsync(customer);
            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Customer>())).
                ReturnsAsync(customerId);

            await _service.UpdateStatusScore(status, customerId);

            _repositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Customer>()), Times.Once);
            _loggerMock.Verify(l =>
               l.Log(LogLevel.Information,
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((v, _) => v.ToString().Contains($"Customer with ID: {customerId} update successfully")),
                   It.IsAny<Exception>(),
                   It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Theory]
        [InlineData(ProcessStatus.Pending, "123")]
        public void UpdateStatusScore_throws_ArgumentNullException_when_id_is_not_found(ProcessStatus status, string customerId)
        {           
            Func<Task> result = async () => await _service.UpdateStatusScore(status, customerId);

            result.Should().ThrowAsync<ArgumentNullException>();
            _repositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

    }
}
