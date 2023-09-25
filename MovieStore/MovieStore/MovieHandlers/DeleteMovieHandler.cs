using FluentResults;
using MediatR;
using MovieStore.Api.Extensions;
using MovieStore.Core.Messages;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.Handlers
{
    public static class DeleteMovie
    {
        public record Command(Guid id) : IRequest<Result>;

        public class DeleteMovieHandler : IRequestHandler<Command, Result>
        {
            private readonly IRepositoryBase<Movie> _repository;

            public DeleteMovieHandler(IRepositoryBase<Movie> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public async Task<Result> Handle(Command request,
            CancellationToken cancellationToken)
            {
                var movie = await _repository.FindById(request.id);
                if (movie == null)
                {
                    return ResultExtensions.FailNotFound(ErrorMessages.NotFoundMovieId());
                }
                _repository.Delete(movie);
                await _repository.SaveChanges();
                return Result.Ok();
            }
        }
    }
}
