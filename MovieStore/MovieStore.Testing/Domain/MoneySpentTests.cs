using FluentAssertions;
using MovieStore.Core.Model;

namespace MovieStore.Testing.Domain
{
    public class MoneySpentTests
    {

        [Theory]
        [InlineData(2)]
        [InlineData(2.42)]
        [InlineData(2.560000)]
        [InlineData(0.1)]
        [InlineData(0.01)]
        public void Create_ShouldReturnResultOk_OnValidAmount(decimal amount)
        {
            var moneySpent = MoneySpent.Create(amount);
            moneySpent.IsSuccess.Should().BeTrue();
            moneySpent.Value.Amount.Should().Be(amount);
        }

        [Theory]
        [InlineData(2.412312)]
        [InlineData(2.4244444)]
        [InlineData(2.560001)]
        [InlineData(0.00001)]
        [InlineData(0.0111111)]
        public void Create_ShouldReturnResultFail_OnInvalidAmount(decimal amount)
        {
            var moneySpent = MoneySpent.Create(amount);
            moneySpent.IsSuccess.Should().BeFalse();
            moneySpent.Errors.ElementAt(0).Message.Should().BeEquivalentTo("Ammount must be at 2 decimals max.");
        }

        [Fact]
        public void Create_ShouldReturnResultFail_OnNegativeAmount()
        {
            var moneySpent = MoneySpent.Create(-2);
            moneySpent.IsSuccess.Should().BeFalse();
            moneySpent.Errors.ElementAt(0).Message.Should().BeEquivalentTo("Ammount must be above or equal to 0.");
        }
    }
}
