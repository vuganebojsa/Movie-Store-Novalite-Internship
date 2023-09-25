using Microsoft.EntityFrameworkCore;
using MovieStore.Api.DTOs;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Infrastructure.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(MovieStoreContext movieStoreContext) : base(movieStoreContext)
        {
        }

        public async Task<PagedList<Customer>> FindByValue(string value, PageDTO page)
        {
            return PagedList<Customer>.ToPagedList(await _movieStoreContext.Customers.Where(
                customer =>
                customer.Name.ToLower().Contains(value.ToLower()) ||
                customer.Email.EmailAddress.ToLower().Contains(value.ToLower())
                ).ToListAsync(),
           page.PageNumber,
           page.PageSize);
        }

        public async Task<int> GetTotalMoviesBought(Guid customerId, int totalDays)
        {
            return await _movieStoreContext.PurchasedMovies.Where(purchasedMovie => purchasedMovie.Customer.Id == customerId && purchasedMovie.DateOfPurchase > DateTime.Now.AddDays(-totalDays)).CountAsync();
        }
    }
}
