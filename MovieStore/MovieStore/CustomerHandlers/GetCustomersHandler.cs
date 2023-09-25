using FluentResults;
using MediatR;
using MovieStore.Api.DTOs;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.Handlers
{
    public static class GetCustomers
    {
        public record Query(PageDTO page) : IRequest<Result<PagedList<PromotedCustomerDTO>>>;

        public class GetCustomersHandler : IRequestHandler<Query, Result<PagedList<PromotedCustomerDTO>>>
        {
            private readonly IRepositoryBase<Customer> _repository;

            public GetCustomersHandler(IRepositoryBase<Customer> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public async Task<Result<PagedList<PromotedCustomerDTO>>> Handle(Query request,
            CancellationToken cancellationToken)
            {
                var customers = await _repository.FindAll(request.page);
                PagedList<PromotedCustomerDTO> returnCustomers = new()
                {
                    TotalCount = customers.TotalCount,
                    PageSize = customers.PageSize,
                    TotalPages = customers.TotalPages
                };

                foreach (var customer in customers)
                {
                    returnCustomers.Add(new PromotedCustomerDTO
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        Email = customer.Email.EmailAddress,
                        MoneySpent = customer.Spent.Amount
                    });
                }
                return Result.Ok(returnCustomers);
            }
        }
    }

}
