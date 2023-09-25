using FluentResults;
using MediatR;
using MovieStore.Core.DTOs;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.Handlers
{
    public static class AddMovie
    {
        public record Command(MovieDTO movie) : IRequest<Result<Movie>>;

        public class AddMovieHandler : IRequestHandler<Command, Result<Movie>>
        {
            private readonly IRepositoryBase<Movie> _repository;

            public AddMovieHandler(IRepositoryBase<Movie> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public async Task<Result<Movie>> Handle(Command request,
            CancellationToken cancellationToken)
            {
                var movie = new Movie { Name = request.movie.Name, LicensingType = request.movie.LicensingType };
                await _repository.Create(movie);
                await _repository.SaveChanges();
                return Result.Ok(movie);
            }
        }
    }
}
