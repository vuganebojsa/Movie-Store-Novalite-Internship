using FakeItEasy;
using FluentAssertions;
using MovieStore.Api.Handlers;
using MovieStore.Core.DTOs;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Testing.MovieHandlers
{
    public class AddMovieHandlerTests
    {

        private readonly AddMovie.AddMovieHandler _movieHandler;
        private readonly IRepositoryBase<Movie> _repository;

        public AddMovieHandlerTests()
        {
            _repository = A.Fake<IRepositoryBase<Movie>>();
            _movieHandler = new AddMovie.AddMovieHandler(_repository);
        }

        [Fact]
        public async Task AddMovie_ShouldReturnOkNewMovie()
        {
            var movieId = Guid.NewGuid();
            var movie = new Movie { LicensingType = Core.Enums.LicensingType.TwoDay, Name = "Harry Pothead" };


            A.CallTo(() => _repository.Create(movie)).Returns(Task.FromResult(movie));

            var command = new AddMovie.Command(new MovieDTO { Name = "Harry Pothead", LicensingType = Core.Enums.LicensingType.TwoDay });

            var result = await _movieHandler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(movie);
        }
    }
}
