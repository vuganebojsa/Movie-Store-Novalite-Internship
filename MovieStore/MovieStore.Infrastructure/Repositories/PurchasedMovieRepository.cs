using Microsoft.EntityFrameworkCore;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Infrastructure.Repositories
{
    public class PurchasedMovieRepository : RepositoryBase<PurchasedMovie>, IPurchasedMovieRepository
    {
        public PurchasedMovieRepository(MovieStoreContext movieStoreContext) : base(movieStoreContext)
        {
        }

        public async Task<PurchasedMovie> GetRentedMovie(Guid movieId)
        {
            var rentedMovie = await _movieStoreContext.PurchasedMovies.Where(
                purchasedMovie => purchasedMovie.Movie.Id == movieId)
                .OrderByDescending(purchasedMovie => purchasedMovie.ExpirationDate.Date).FirstOrDefaultAsync();
            return rentedMovie;
        }

        public async Task<PurchasedMovie> GetPurhasedMovie(Guid movieId)
        {
            var purchasedMovie = await _movieStoreContext.PurchasedMovies.Where(
                purchasedMovie => purchasedMovie.Movie.Id == movieId).FirstOrDefaultAsync();
            return purchasedMovie;
        }

        public async Task<ICollection<PurchasedMovie>> GetExpiredPurchasedMovies()
        {
            var purchasedMovies = await _movieStoreContext.PurchasedMovies.Where(
                purchasedMovie => purchasedMovie.Movie.LicensingType == Core.Enums.LicensingType.TwoDay && purchasedMovie.ExpirationDate != null
                ).Include(purchasedMovie => purchasedMovie.Movie)
                .Include(purchasedMovie => purchasedMovie.Customer).ToListAsync();
            var filteredPurchasedMovies = new List<PurchasedMovie>();
            foreach (var movie in purchasedMovies)
            {
                TimeSpan diff = (TimeSpan)(movie.ExpirationDate.Date - movie.DateOfPurchase);
                if ((int)diff.Days > 2) filteredPurchasedMovies.Add(movie);
            }
            return filteredPurchasedMovies;
        }
    }
}
