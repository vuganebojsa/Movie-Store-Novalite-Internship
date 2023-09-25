using Microsoft.EntityFrameworkCore;
using MovieStore.Api.DTOs;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Infrastructure.Repositories
{
    public class MovieRepository : RepositoryBase<Movie>, IMovieRepository
    {
        public MovieRepository(MovieStoreContext movieStoreContext) : base(movieStoreContext)
        {
        }
        public async Task<PagedList<Movie>> FindByValue(string value, PageDTO page)
        {
            return PagedList<Movie>.ToPagedList(await _movieStoreContext.Movies.Where(
                movie =>
                movie.Name.ToLower().Contains(value.ToLower())
                ).ToListAsync(),
           page.PageNumber,
           page.PageSize);
        }

    }
}
