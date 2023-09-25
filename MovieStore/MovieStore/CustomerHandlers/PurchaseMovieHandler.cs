using FluentResults;
using MediatR;
using Microsoft.Extensions.Options;
using MovieStore.Api.DTOs;
using MovieStore.Api.Extensions;
using MovieStore.Api.Options;
using MovieStore.Core.Enums;
using MovieStore.Core.Messages;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.CustomerHandlers
{
    public static class PurchaseMovie
    {
        public record Query(Guid customerId, Guid movieId) : IRequest<Result<NewPurchasedMovieDTO>>;

        public class PurchaseMovieHandler : IRequestHandler<Query, Result<NewPurchasedMovieDTO>>
        {
            private readonly IRepositoryBase<Customer> _customerRepository;
            private readonly IRepositoryBase<Movie> _movieRepository;
            private readonly IPurchasedMovieRepository _purchasedMovieRepository;
            private readonly MoviePriceOptions _moviePriceOptions;

            public PurchaseMovieHandler(IRepositoryBase<Customer> customerRepository, IRepositoryBase<Movie> movieRepository, IPurchasedMovieRepository purchasedMovieRepository, IOptions<MoviePriceOptions> moviePriceOptions)
            {
                _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
                _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
                _purchasedMovieRepository = purchasedMovieRepository ?? throw new ArgumentNullException(nameof(purchasedMovieRepository));
                _moviePriceOptions = moviePriceOptions.Value;
            }
            public async Task<Result<NewPurchasedMovieDTO>> Handle(Query request,
            CancellationToken cancellationToken)
            {
                var customer = await _customerRepository.FindById(request.customerId);
                if (customer is null)
                {
                    return ResultExtensions.FailNotFound<NewPurchasedMovieDTO>(ErrorMessages.NotFoundCustomerId());

                }
                var movie = await _movieRepository.FindById(request.movieId);
                if (movie == null)
                {
                    return ResultExtensions.FailNotFound<NewPurchasedMovieDTO>(ErrorMessages.NotFoundMovieId());
                }

                bool isPurchasable;
                // if movie rented check if it is in last two days
                if (movie.IsForRent())
                    isPurchasable = await IsMovieRentable(movie);
                else
                {
                    isPurchasable = await IsMoviePurchasable(movie);
                }
                if (!isPurchasable)
                {
                    return ResultExtensions.FailBadRequest<NewPurchasedMovieDTO>(ErrorMessages.MovieNotRentable());
                }

                var newPurchasedMovie = await SaveNewPurchasedMovie(customer, movie);
                if (newPurchasedMovie.IsSuccess) return newPurchasedMovie;
                return ResultExtensions.FailBadRequest<NewPurchasedMovieDTO>(newPurchasedMovie.Errors.ElementAt(0).Message);
            }

            private async Task<Result<NewPurchasedMovieDTO>> SaveNewPurchasedMovie(Customer customer, Movie movie)
            {

                decimal price = CalculatePrice(customer.Status, movie.LicensingType);
                var validPrice = MoneySpent.Create(price);
                if (!validPrice.IsSuccess) return Result.Fail<NewPurchasedMovieDTO>(ErrorMessages.InvalidPrice());

                PurchasedMovie purchasedMovie = CreatePurchasedMovie(movie, validPrice, customer);
                var purchase = customer.PurchaseMovie(purchasedMovie);
                if (purchase.IsFailed) return Result.Fail<NewPurchasedMovieDTO>(purchase.Errors.ElementAt(0).Message);
                await _customerRepository.SaveChanges();
                return Result.Ok(GetPurchasedMovieDTo(customer, movie, price));
            }

            private static PurchasedMovie CreatePurchasedMovie(Movie movie, Result<MoneySpent> validPrice, Customer customer)
            {
                return PurchasedMovieFactory.Create(movie, validPrice.Value, customer);
            }

            private static NewPurchasedMovieDTO GetPurchasedMovieDTo(Customer customer, Movie movie, decimal price)
            {
                return new NewPurchasedMovieDTO { CustomerId = customer.Id, MovieId = movie.Id, ExpirationDate = movie.IsForRent() ? DateTime.Now.AddDays(2) : null, Price = price };
            }

            private decimal CalculatePrice(Status status, LicensingType licensingType)
            {
                decimal advancedDiscount = 0;
                if (status == Status.Advanced) advancedDiscount = _moviePriceOptions.AdvancedDiscount;
                decimal basePrice = licensingType == LicensingType.TwoDay ? _moviePriceOptions.TwoDayPrice : _moviePriceOptions.LifelongPrice;

                decimal total = basePrice - basePrice * advancedDiscount;
                return total;
            }

            private async Task<bool> IsMoviePurchasable(Movie movie)
            {
                var purchasedMovieExist = await _purchasedMovieRepository.GetPurhasedMovie(movie.Id);
                return purchasedMovieExist == null;
            }

            private async Task<bool> IsMovieRentable(Movie movie)
            {
                var rentedMovie = await _purchasedMovieRepository.GetRentedMovie(movie.Id);
                if (rentedMovie == null) return true;
                if (rentedMovie.ExpirationDate == null) return true;
                return rentedMovie.ExpirationDate.Date < DateTime.Now;
            }
        }
    }
}
