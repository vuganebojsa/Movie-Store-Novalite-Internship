using FluentAssertions;
using MovieStore.Core.Model;

namespace MovieStore.Testing.Domain
{
    public class CustomerTests
    {
        [Fact]
        public void Create_ShouldReturnNewCustomer()
        {
            var customer = Customer.Create("Nebojsa vuga", Email.Create("nebojsa01@gmail.com").Value);

            customer.Should().NotBeNull();
            customer.Expiration.Date.Should().Be(null);
            customer.Role.Should().Be(Core.Enums.Role.Regular);
            customer.Status.Should().Be(Core.Enums.Status.Regular);
            customer.Spent.Should().Be(MoneySpent.Of(0));
        }

        [Fact]
        public void UpgradeStatus_ShouldReturnResultOk_OnValidStatus()
        {
            var customer = Customer.Create("Nebojsa vuga", Email.Create("nebojsa01@gmail.com").Value);

            var upgrade = customer.UpgradeStatus();
            upgrade.IsSuccess.Should().BeTrue();
            customer.Status.Should().Be(Core.Enums.Status.Advanced);
            customer.Expiration.Date.Should().BeAfter(DateTime.Now.AddDays(364));

        }

        [Fact]
        public void UpgradeStatus_ShouldReturnResultFail_OnInvalidStatus()
        {
            var customer = Customer.Create("Nebojsa vuga", Email.Create("nebojsa01@gmail.com").Value);

            var upgrade = customer.UpgradeStatus();
            customer.Status.Should().Be(Core.Enums.Status.Advanced);

            upgrade = customer.UpgradeStatus();

            upgrade.IsSuccess.Should().BeFalse();
            upgrade.Errors.ElementAt(0).Message.Should().BeEquivalentTo("The customer status is already advanced.");

        }

        [Fact]
        public void PurchaseMovie_ShouldReturnResultOk_OnValidReferences()
        {
            var customer = Customer.Create("Nebojsa vuga", Email.Create("nebojsa01@gmail.com").Value);
            var purchasedMovie = PurchasedMovieFactory.Create(new Movie { Id = new Guid(), LicensingType = Core.Enums.LicensingType.TwoDay, Name = "Movie" }, MoneySpent.Of(5), customer);

            customer.PurchasedMovies.Count.Should().Be(0);

            var purchase = customer.PurchaseMovie(purchasedMovie);
            customer.PurchasedMovies.Count.Should().Be(1);
            purchase.IsSuccess.Should().BeTrue();


        }
        [Fact]
        public void PurchaseMovie_ShouldReturnResultFail_OnInvalidReferences()
        {
            var customer = Customer.Create("Nebojsa vuga", Email.Create("nebojsa01@gmail.com").Value);
            var purchasedMovie = PurchasedMovieFactory.Create(new Movie { Id = new Guid(), LicensingType = Core.Enums.LicensingType.TwoDay, Name = "Movie" }, MoneySpent.Of(5), customer);

            var otherCustomer = Customer.Create("Nebojsa vuga", Email.Create("nebojsa01088@gmail.com").Value);

            otherCustomer.PurchasedMovies.Count.Should().Be(0);

            var purchase = otherCustomer.PurchaseMovie(purchasedMovie);
            customer.PurchasedMovies.Count.Should().Be(0);
            purchase.IsSuccess.Should().BeFalse();
            purchase.Errors.ElementAt(0).Message.Should().BeEquivalentTo("Invalid customer reference.");


        }
    }
}
