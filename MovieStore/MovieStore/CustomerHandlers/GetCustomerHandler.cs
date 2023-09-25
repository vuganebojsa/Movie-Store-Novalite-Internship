using FluentResults;
using MediatR;
using MovieStore.Api.DTOs;
using MovieStore.Api.Extensions;
using MovieStore.Core.Messages;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.Handlers
{
    public static class GetCustomer
    {
        public record Query(Guid id) : IRequest<Result<PromotedCustomerDTO>>;
        public class GetCustomerHandler : IRequestHandler<Query, Result<PromotedCustomerDTO>>
        {
            private readonly IRepositoryBase<Customer> _repository;

            public GetCustomerHandler(IRepositoryBase<Customer> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public async Task<Result<PromotedCustomerDTO>> Handle(Query request,
            CancellationToken cancellationToken)
            {
                var customer = await _repository.FindById(request.id);
                return customer is null ?
                    ResultExtensions.FailNotFound(ErrorMessages.NotFound("Customer", "Id"))
                    : Result.Ok(new PromotedCustomerDTO
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        Email = customer.Email.EmailAddress,
                        MoneySpent = customer.Spent.Amount
                    });

            }
        }
    }
}
