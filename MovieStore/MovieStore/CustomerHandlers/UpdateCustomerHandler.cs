using FluentResults;
using MediatR;
using MovieStore.Api.Extensions;
using MovieStore.Core.DTOs;
using MovieStore.Core.Messages;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.Handlers
{
    public static class UpdateCustomer
    {
        public record Query(Guid id, Core.DTOs.CustomerDTO customer) : IRequest<Result<Core.Model.Customer>>;

        public class UpdateCustomerHandler : IRequestHandler<Query, Result<Core.Model.Customer>>
        {
            private readonly IRepositoryBase<Core.Model.Customer> _repository;

            public UpdateCustomerHandler(IRepositoryBase<Core.Model.Customer> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public async Task<Result<Core.Model.Customer>> Handle(Query request,
            CancellationToken cancellationToken)
            {
                var customer = await _repository.FindById(request.id);
                if (customer is null)
                    return ResultExtensions.FailNotFound<Core.Model.Customer>(ErrorMessages.NotFound("Customer", "Id"));
                customer.Name = request.customer.Name;
                await _repository.SaveChanges();
                return Result.Ok(customer);
            }
        }
    }
}
