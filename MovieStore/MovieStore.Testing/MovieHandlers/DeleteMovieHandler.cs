using FakeItEasy;
using FluentAssertions;
using MovieStore.Api.Handlers;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Testing.MovieHandlers
{
    public class DeleteMovieHandler
    {
        private readonly DeleteMovie.DeleteMovieHandler _movieHandler;
        private readonly IRepositoryBase<Movie> _repository;

        public DeleteMovieHandler()
        {
            _repository = A.Fake<IRepositoryBase<Movie>>();
            _movieHandler = new DeleteMovie.DeleteMovieHandler(_repository);
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsResultOk()
        {
            var movieId = Guid.NewGuid();
            var expectedMovie = new Movie { Id = movieId, Name = "Test", LicensingType = Core.Enums.LicensingType.TwoDay };
            A.CallTo(() => _repository.FindById(movieId)).Returns(expectedMovie);
            var query = new DeleteMovie.Command(movieId);

            var result = await _movieHandler.Handle(query, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_InvalidId_ReturnsFailFromResult()
        {
            var movieId = Guid.NewGuid();

            A.CallTo(() => _repository.FindById(movieId)).Returns(Task.FromResult<Movie>(null));
            var query = new DeleteMovie.Command(movieId);

            var result = await _movieHandler.Handle(query, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
            result.Errors.ElementAt(0).Message.Should().BeEquivalentTo("The movie with the given id doesn't exist.");
        }
    }
}
