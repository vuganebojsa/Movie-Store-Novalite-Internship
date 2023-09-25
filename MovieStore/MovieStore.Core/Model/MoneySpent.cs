using FluentResults;
using MovieStore.Core.Messages;

namespace MovieStore.Core.Model
{
    public record MoneySpent
    {
        public decimal Amount { get; init; } = 0;
        private MoneySpent(decimal ammount) => Amount = ammount;
        private MoneySpent() { }
        public static Result<MoneySpent> Create(decimal ammount)
        {
            if (decimal.Round(ammount, 2) != ammount)
            {
                return Result.Fail(ErrorMessages.DecimalNumberAmount());
            }
            return ammount >= 0 ? Result.Ok<MoneySpent>(new(ammount)) : Result.Fail(ErrorMessages.DecimalValueAmount());
        }

        public static MoneySpent Of(decimal amount) => new(amount);
        public static bool operator <(MoneySpent obj1, decimal obj2) => obj1.Amount < obj2;
        public static bool operator >(MoneySpent obj1, decimal obj2) => obj1.Amount > obj2;

    }
}
