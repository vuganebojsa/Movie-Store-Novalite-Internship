namespace MovieStore.Api.DTOs
{
    public class NewPurchasedMovieDTO
    {
        public Guid MovieId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public decimal Price { get; set; } = 0;
    }
}
