using FakeItEasy;
using FluentAssertions;
using MovieStore.Api.Handlers;
using MovieStore.Core.DTOs;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Testing.MovieHandlers
{
    public class UpdateMovieHandlerTests
    {
        private readonly UpdateMovie.UpdateMovieHandler _movieHandler;
        private readonly IRepositoryBase<Movie> _repository;

        public UpdateMovieHandlerTests()
        {
            _repository = A.Fake<IRepositoryBase<Movie>>();
            _movieHandler = new UpdateMovie.UpdateMovieHandler(_repository);
        }

        [Fact]
        public async Task UpdateMovie_ShouldReturnUpdatedMovie()
        {
            var movieId = Guid.NewGuid();
            var movie = new Movie { Id = movieId, LicensingType = Core.Enums.LicensingType.TwoDay, Name = "Harry Pothead" };
            var returnedMovie = new Movie { Name = "Harry Plopper", LicensingType = Core.Enums.LicensingType.TwoDay, Id = movieId };

            var command = new UpdateMovie.Command(movieId, new MovieDTO { Name = "Harry Plopper", LicensingType = Core.Enums.LicensingType.TwoDay });

            A.CallTo(() => _repository.FindById(movieId)).Returns(returnedMovie);

            // act
            var result = await _movieHandler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(returnedMovie);
        }

        [Fact]
        public async Task UpdateMovie_OnInvalidId_ShouldReturnResultFail()
        {
            var movieId = Guid.NewGuid();

            var command = new UpdateMovie.Command(movieId, new MovieDTO { Name = "Harry Plopper", LicensingType = Core.Enums.LicensingType.TwoDay });

            A.CallTo(() => _repository.FindById(movieId)).Returns(Task.FromResult<Movie>(null));

            // act
            var result = await _movieHandler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
            result.Errors.ElementAt(0).Should().NotBeNull();
        }
    }
}
