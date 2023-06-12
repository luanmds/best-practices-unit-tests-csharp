using BestPractices.Domain.DTOs;
using FluentAssertions;
using Xunit;

namespace BestPractices.GoodTests.DTOs
{
    public class CustomerDtoTests : IClassFixture<CustomerDto>
    {
        private readonly CustomerDto _dto;

        public CustomerDtoTests(CustomerDto dto)
        {
            _dto = dto;
        }

        [Fact]
        public void ToCustomer_should_returns_Customer_with_Dto_data()
        {
            var sut = _dto;

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
