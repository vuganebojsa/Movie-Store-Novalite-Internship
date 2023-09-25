using MovieStore.Api.DTOs;
using MovieStore.Core.Model;

namespace MovieStore.Infrastructure.Interfaces
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Task<PagedList<Customer>> FindByValue(string value, PageDTO page);
        Task<int> GetTotalMoviesBought(Guid customerId, int totalDays);
    }
}
