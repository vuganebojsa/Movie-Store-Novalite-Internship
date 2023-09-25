using FluentResults;
using MediatR;
using MovieStore.Api.Extensions;
using MovieStore.Core.DTOs;
using MovieStore.Core.Messages;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.Handlers
{
    public static class UpdateMovie
    {
        public record Command(Guid id, MovieDTO movie) : IRequest<Result<Movie>>;

        public class UpdateMovieHandler : IRequestHandler<Command, Result<Movie>>
        {
            private readonly IRepositoryBase<Movie> _repository;

            public UpdateMovieHandler(IRepositoryBase<Movie> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public async Task<Result<Movie>> Handle(Command request,
            CancellationToken cancellationToken)
            {
                var movie = await _repository.FindById(request.id);
                if (movie == null)
                {
                    return ResultExtensions.FailNotFound(ErrorMessages.NotFoundMovieId());
                }
                await SaveMovie(request, movie);
                return Result.Ok(movie);
            }

            private async Task SaveMovie(Command request, Movie movie)
            {
                movie.LicensingType = request.movie.LicensingType;
                movie.Name = request.movie.Name;
                await _repository.SaveChanges();
            }
        }
    }
}
