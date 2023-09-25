namespace MovieStore.Core.Model
{
    public class PurchasedMovie
    {
        public Guid Id { get; set; }
        public Movie Movie { get; set; }
        public Customer Customer { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public ExpirationDate ExpirationDate { get; set; } = ExpirationDate.Infinity;
        public MoneySpent Price { get; set; }
        internal PurchasedMovie()
        {
        }
    }
}
