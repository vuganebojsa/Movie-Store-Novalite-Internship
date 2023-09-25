using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Api.Controllers;
using MovieStore.Api.CustomerHandlers;
using MovieStore.Api.DTOs;
using MovieStore.Api.Handlers;
using MovieStore.Core.Model;

namespace CustomerStore.Api.Controllers
{

    [Route("api/v1/customers")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<PromotedCustomerDTO>), 200)]
        public async Task<IActionResult> GetCustomers([FromQuery] PageDTO page)
        {
            var customers = await _mediator.Send(new GetCustomers.Query(page));
            if (customers.IsSuccess)
            {
                SetMetadata(customers.Value);
            }
            return GetResult(customers);
        }
        [HttpGet("search/{value}")]
        [ProducesResponseType(typeof(PagedList<PromotedCustomerDTO>), 200)]
        public async Task<IActionResult> FindCustomer(string value, [FromQuery] PageDTO page)
        {
            var customers = await _mediator.Send(new FindCustomers.Query(value, page));
            if (customers.IsSuccess)
            {
                SetMetadata(customers.Value);
            }
            return GetResult(customers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PromotedCustomerDTO), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomer.Query(id));
            return GetResult(customer);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> AddCustomer([FromBody] MovieStore.Core.DTOs.CustomerDTO customer)
        {
            var newCustomer = await _mediator.Send(new AddCustomer.Command(customer));
            return GetResult(newCustomer);

        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] MovieStore.Core.DTOs.CustomerDTO customer)
        {
            var newCustomer = await _mediator.Send(new UpdateCustomer.Query(id, customer));
            return GetResult(newCustomer);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 204)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _mediator.Send(new DeleteCustomer.Command(id));
            return result.IsSuccess ? NoContent() : NotFound(result.Errors.ElementAt(0).Message);
        }

        [HttpPut("{customerId}/purchase/{movieId}")]
        [ProducesResponseType(typeof(NewPurchasedMovieDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> PurchaseMovie(Guid customerId, Guid movieId)
        {
            var newPurchasedMovie = await _mediator.Send(new PurchaseMovie.Query(customerId, movieId));
            return GetResult(newPurchasedMovie);
        }

        [HttpPut("{id}/promote")]
        [ProducesResponseType(typeof(PromotedCustomerDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> Promote(Guid id)
        {
            var promotedCustomer = await _mediator.Send(new PromoteCustomer.Query(id));
            return GetResult(promotedCustomer);
        }
    }
}
