namespace MovieStore.Core.Model
{
    public static class PurchasedMovieFactory
    {
        public static PurchasedMovie Create(Movie movie, MoneySpent price, Customer customer)
        {
            return new PurchasedMovie
            {
                Movie = movie,
                DateOfPurchase = DateTime.Now,
                ExpirationDate = movie.IsForRent() ? new ExpirationDate(DateTime.Now.AddDays(2)) : ExpirationDate.Infinity,
                Price = price,
                Customer = customer
            };
        }
    }
}
