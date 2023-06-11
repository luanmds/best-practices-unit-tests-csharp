using AutoFixture;
using BestPractices.Domain.DTOs;
using FluentAssertions;
using Xunit;

namespace BestPractices.GoodTests.DTOs
{
    public class CustomerDtoTests
    {
        private readonly Fixture _fixture;

        public CustomerDtoTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ToCustomer_should_returns_Customer_with_Dto_data()
        {
            var sut = _fixture.Create<CustomerDto>();

            var result = sut.ToCustomer();

            result.Address.Street.Should().Be(sut.Street);
            result.Address.City.Should().Be(sut.City);
            result.Address.StreetNumber.Should().Be(sut.StreetNumber);
            result.Name.Should().Be(sut.Name);
            result.Age.Should().Be(sut.Age);
            result.Document.Should().Be(sut.Document);
            result.DebitsAmount.Should().Be(sut.DebitsAmount);
        }
    }
}
