namespace MovieStore.Core.DTOs
{
    public class PurchasedMovieDTO
    {
        public Guid MovieId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
