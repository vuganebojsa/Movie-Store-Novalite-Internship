using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Api.DTOs;
using MovieStore.Api.Handlers;
using MovieStore.Api.MovieHandlers;
using MovieStore.Core.DTOs;
using MovieStore.Core.Model;

namespace MovieStore.Api.Controllers
{
    [Route("api/v1/movies")]
    [ApiController]
    public class MovieController : BaseController
    {

        private readonly IMediator _mediator;

        public MovieController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<Movie>), 200)]
        public async Task<IActionResult> GetMovies([FromQuery] PageDTO page)
        {
            var response = await _mediator.Send(new GetMovies.Query(page));
            if (response.IsSuccess)
            {
                SetMetadata(response.Value);
            }
            return GetResult(response);
        }
        [HttpGet("search/{value}")]
        [ProducesResponseType(typeof(PagedList<Movie>), 200)]
        public async Task<IActionResult> FindMovie(string value, [FromQuery] PageDTO page)
        {
            var movies = await _mediator.Send(new FindMovies.Query(value, page));
            if (movies.IsSuccess)
            {
                SetMetadata(movies.Value);
            }
            return GetResult(movies);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Movie), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetMovie(Guid id)
        {
            var response = await _mediator.Send(new GetMovie.Query(id));
            return GetResult(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Movie), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> AddMovie([FromBody] MovieDTO movie)
        {
            var newMovie = await _mediator.Send(new AddMovie.Command(movie));
            return GetResult(newMovie);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Movie), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> UpdateMovie(Guid id, [FromBody] MovieDTO movie)
        {
            var updatedMovie = await _mediator.Send(new UpdateMovie.Command(id, movie));
            return GetResult(updatedMovie);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 204)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            var result = await _mediator.Send(new DeleteMovie.Command(id));
            return result.IsSuccess ? NoContent() : NotFound(result.Errors.ElementAt(0).Message);
        }
    }
}
