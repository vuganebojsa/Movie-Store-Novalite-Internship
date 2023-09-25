namespace MovieStore.Api.Options
{
    public class CustomerPromotionOptions
    {
        public int MinimumNumberOfMoviesBought { get; set; }
        public int Days { get; set; }
        public decimal MinimumMoneySpent { get; set; }
    }
}
