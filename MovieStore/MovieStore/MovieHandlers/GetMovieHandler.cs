using FluentResults;
using MediatR;
using MovieStore.Api.Extensions;
using MovieStore.Core.Messages;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.Handlers
{
    public static class GetMovie
    {
        public record Query(Guid id) : IRequest<Result<Movie>>;
        public class GetMovieHandler : IRequestHandler<Query, Result<Movie>>
        {
            private readonly IRepositoryBase<Movie> _repository;

            public GetMovieHandler(IRepositoryBase<Movie> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public async Task<Result<Movie>> Handle(Query request,
            CancellationToken cancellationToken)
            {
                var movie = await _repository.FindById(request.id);
                return movie == null ? ResultExtensions.FailNotFound(ErrorMessages.NotFoundMovieId()) : Result.Ok(movie);
            }
        }
    }
}
