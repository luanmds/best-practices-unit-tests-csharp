﻿using AutoFixture;
using BestPractices.Domain.Entities;
using BestPractices.Domain.Repositories.Interfaces;
using BestPractices.Domain.Services;
using FluentAssertions;
using Moq;
using System.Collections;
using Xunit;

namespace BestPractices.GoodTests.Services
{
    public class UsingTheoryWithClassData
    {
        private readonly Mock<ILoanRepository> _repositoryMock;
        private readonly Mock<IScoreRepository> _scoreRepositoryMock;
        private readonly Fixture _fixture;
        private readonly LoanService _service;

        public UsingTheoryWithClassData()
        {
            _repositoryMock = new Mock<ILoanRepository>();
            _scoreRepositoryMock = new Mock<IScoreRepository>();
            _fixture = new Fixture();
            _service = new LoanService(_repositoryMock.Object, _scoreRepositoryMock.Object);
        }

        [Fact]
        public async void CreateLoan_successfully_when_score_exists()
        {
            var score = _fixture.Create<Score>();
            var dt = DateTime.UtcNow;
            _scoreRepositoryMock.Setup(x => x.GetAsync(score.Id))
                .ReturnsAsync(score);

            await _service.CreateLoan(score.Id, dt);

            _scoreRepositoryMock.Verify(x => x.GetAsync(score.Id), Times.Once);
            _repositoryMock.Verify(x => x.SaveAsync(It.IsAny<Loan>()), Times.Once);
        }

        [Fact]
        public void CreateLoan_throws_ArgumentNullException_when_score_not_found()
        {
            var id = _fixture.Create<string>();
            var dt = DateTime.UtcNow;

            Func<Task> result = async () => await _service.CreateLoan(id, dt);

            result.Should().ThrowAsync<ArgumentNullException>();
            _scoreRepositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(x => x.SaveAsync(It.IsAny<Loan>()), Times.Never);
        }

        [Theory]
        [ClassData(typeof(CalculateLoanValueData))]
        public void CalculateLoanValue_should_return_correct_value(decimal debits, int score, decimal expectedLoan)
        {
            decimal result = _service.CalculateLoanValue(debits, score);

            result.Should().Be(expectedLoan);
        }

        [Theory]
        [InlineData(0, 19.3)]
        [InlineData(90, 12.5)]
        [InlineData(100, 12.5)]                
        [InlineData(89, 16)]
        [InlineData(51, 16)]
        public void CalculateFeesByMonth_should_return_correct_value_based_score(int score, double expectedValue)
        {
            double result = _service.CalculateFeesByMonth(score);

            result.Should().Be(expectedValue);
        }
    }

    public class CalculateLoanValueData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 2000f, 0, 0M };
            yield return new object[] { 450f, -55, 0 };
            yield return new object[] { 500.3f, 50, 4499.7 };
            yield return new object[] { 250.9f, 75, 7249.1 };
            yield return new object[] { 0f, 100, 10000 };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
