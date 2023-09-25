using FluentResults;
using MediatR;
using MovieStore.Api.DTOs;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.MovieHandlers
{
    public static class FindMovies
    {
        public record Query(string value, PageDTO page) : IRequest<Result<PagedList<Movie>>>;
        public class FindMoviesHandler : IRequestHandler<Query, Result<PagedList<Movie>>>
        {
            private readonly IMovieRepository _repository;

            public FindMoviesHandler(IMovieRepository repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public async Task<Result<PagedList<Movie>>> Handle(Query request,
            CancellationToken cancellationToken)
            {
                PagedList<Movie> movies = await _repository.FindByValue(request.value, request.page);
                return Result.Ok(movies);
            }
        }
    }
}
