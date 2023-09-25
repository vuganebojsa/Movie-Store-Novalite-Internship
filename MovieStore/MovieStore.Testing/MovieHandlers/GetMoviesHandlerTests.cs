using FakeItEasy;
using FluentAssertions;
using MovieStore.Api.Handlers;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Testing.MovieHandlers
{
    public class GetMoviesHandlerTests
    {
        private readonly GetMovies.GetMoviesHandler _movieHandler;
        private readonly IRepositoryBase<Movie> _repository;

        public GetMoviesHandlerTests()
        {
            _repository = A.Fake<IRepositoryBase<Movie>>();
            _movieHandler = new GetMovies.GetMoviesHandler(_repository);
        }

        [Fact]
        public async Task Handle_ReturnsMovies()
        {
            var movies = new List<Movie>()
            {
                new Movie { Id = Guid.NewGuid(), Name = "Test", LicensingType = Core.Enums.LicensingType.TwoDay },
                new Movie { Id = Guid.NewGuid(), Name = "Test", LicensingType = Core.Enums.LicensingType.TwoDay }
            };

            A.CallTo(() => _repository.FindAll()).Returns(movies);
            var query = new GetMovies.Query();

            var result = await _movieHandler.Handle(query, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Count.Should().Be(2);
        }
    }
}
