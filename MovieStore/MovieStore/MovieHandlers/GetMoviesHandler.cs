using FluentResults;
using MediatR;
using MovieStore.Api.DTOs;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.Handlers
{
    public static class GetMovies
    {
        public record Query(PageDTO page) : IRequest<Result<PagedList<Movie>>>;

        public class GetMoviesHandler : IRequestHandler<Query, Result<PagedList<Movie>>>
        {
            private readonly IRepositoryBase<Movie> _repository;

            public GetMoviesHandler(IRepositoryBase<Movie> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public async Task<Result<PagedList<Movie>>> Handle(Query request,
            CancellationToken cancellationToken)
            {
                var movies = await _repository.FindAll(request.page);
                return Result.Ok(movies);
            }
        }
    }

}
