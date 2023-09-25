using FluentResults;
using MovieStore.Core.Enums;
using MovieStore.Core.Messages;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStore.Core.Model
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public Email Email { get; private set; }
        public Status Status { get; private set; }
        public ExpirationDate Expiration { get; private set; } = ExpirationDate.Infinity;
        public Role Role { get; private set; }
        public MoneySpent Spent { get; private set; } = MoneySpent.Of(0);
        [NotMapped]
        private readonly List<PurchasedMovie> _purchasedMovies = new();

        public IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies;

        public Result PurchaseMovie(PurchasedMovie purchasedMovie)
        {
            if (purchasedMovie.Customer != this) return Result.Fail(ErrorMessages.InvalidCustomerReference());
            _purchasedMovies.Add(purchasedMovie);
            Spent = MoneySpent.Of(Spent.Amount + purchasedMovie.Price.Amount);
            return Result.Ok();
        }

        public Result UpgradeStatus()
        {
            if (Status == Status.Advanced) return Result.Fail(ErrorMessages.CannotPromoteReasonAlreadyAdvanced());
            Status = Status.Advanced;
            Expiration = new ExpirationDate(DateTime.Now.AddYears(1));
            return Result.Ok();
        }

        public static Customer Create(string name, Email email)
        {
            return new Customer
            {
                Name = name,
                Email = email,
                Expiration = ExpirationDate.Infinity,
                Role = Role.Regular,
                Status = Status.Regular,
                Spent = MoneySpent.Of(0)
            };
        }

        public static bool operator ==(Customer left, Customer right) => left.Id == right.Id && left.Email == right.Email;
        public static bool operator !=(Customer left, Customer right) => left.Id != right.Id || left.Email != right.Email;
    }
}
