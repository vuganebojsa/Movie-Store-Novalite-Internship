using FakeItEasy;
using FluentAssertions;
using MovieStore.Api.Handlers;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Testing.MovieHandlers
{
    public class GetMovieHandlerTests
    {
        private readonly GetMovie.GetMovieHandler _movieHandler;
        private readonly IRepositoryBase<Movie> _repository;

        public GetMovieHandlerTests()
        {
            _repository = A.Fake<IRepositoryBase<Movie>>();
            _movieHandler = new GetMovie.GetMovieHandler(_repository);
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsMovie()
        {
            var movieId = Guid.NewGuid();
            var expectedMovie = new Movie { Id = movieId, Name = "Test", LicensingType = Core.Enums.LicensingType.TwoDay };
            A.CallTo(() => _repository.FindById(movieId)).Returns(expectedMovie);
            var query = new GetMovie.Query(movieId);

            var result = await _movieHandler.Handle(query, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(expectedMovie);
        }

        [Fact]
        public async Task Handle_InvalidId_ReturnsNotFoundResult()
        {
            var movieId = Guid.NewGuid();

            A.CallTo(() => _repository.FindById(movieId)).Returns(Task.FromResult<Movie>(null));
            var query = new GetMovie.Query(movieId);

            var result = await _movieHandler.Handle(query, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
            result.Errors.ElementAt(0).Message.Should().BeEquivalentTo("The movie with the given id doesn't exist.");
        }
    }
}
