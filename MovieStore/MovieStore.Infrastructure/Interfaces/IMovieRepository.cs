using MovieStore.Api.DTOs;
using MovieStore.Core.Model;

namespace MovieStore.Infrastructure.Interfaces
{
    public interface IMovieRepository : IRepositoryBase<Movie>
    {
        Task<PagedList<Movie>> FindByValue(string value, PageDTO page);

    }
}
