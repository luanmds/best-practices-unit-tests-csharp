using AutoFixture;
using BestPractices.Domain.Entities;
using BestPractices.Domain.Repositories.Interfaces;
using BestPractices.Domain.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BestPractices.GoodTests.Services
{
    public class ScoreServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IScoreRepository> _repositoryMock;
        private readonly Mock<ILogger<ScoreService>> _loggerMock;
        private readonly Fixture _fixture;
        private readonly ScoreService _service;

        public ScoreServiceTests()
        {
            _repositoryMock = new Mock<IScoreRepository>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _loggerMock = new Mock<ILogger<ScoreService>>();
            _fixture = new Fixture();
            _service = new ScoreService(_repositoryMock.Object, _customerRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async void CreateScore_successfully_when_customer_exists()
        {
            var customer = _fixture.Create<Customer>();
            _customerRepositoryMock.Setup(x => x.GetAsync(customer.Id))
                .ReturnsAsync(customer);

            await _service.CreateScore(customer.Id);

            _customerRepositoryMock.Verify(x => x.GetAsync(customer.Id), Times.Once);
            _repositoryMock.Verify(x => x.SaveAsync(It.IsAny<Score>()), Times.Once);
            _loggerMock.Verify(l =>
              l.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                      It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Score with ID:")),
                      It.IsAny<Exception>(),
                      It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public void CreateScore_throws_ArgumentNullException_when_customer_not_found()
        {
            var id = _fixture.Create<string>();

            Func<Task> result = async () => await _service.CreateScore(id);

            result.Should().ThrowAsync<ArgumentNullException>();
            _customerRepositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(x => x.SaveAsync(It.IsAny<Score>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(CalculateScoreData))]
        public void CalculateScore_should_return_correct_value_to_the_debits_amount(decimal debits, int expectedScore)
        {
            int score = ScoreService.CalculateScore(debits);

            score.Should().Be(expectedScore);
        }

        public static IEnumerable<object[]> CalculateScoreData()
        {
            yield return new object[] { 2000f, 0 };
            yield return new object[] { 1000.0f, 0 };
            yield return new object[] { 500.3f, 50 };
            yield return new object[] { 450f, 55 };
            yield return new object[] { 250.9f, 75 };
            yield return new object[] { 0f, 100 };
        }
    }
}
