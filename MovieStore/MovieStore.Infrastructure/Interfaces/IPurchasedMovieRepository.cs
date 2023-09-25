using MovieStore.Core.Model;

namespace MovieStore.Infrastructure.Interfaces
{
    public interface IPurchasedMovieRepository : IRepositoryBase<PurchasedMovie>
    {
        Task<PurchasedMovie> GetRentedMovie(Guid movieId);
        Task<PurchasedMovie> GetPurhasedMovie(Guid movieId);
        Task<ICollection<PurchasedMovie>> GetExpiredPurchasedMovies();
    }
}
