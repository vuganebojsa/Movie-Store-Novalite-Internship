﻿using FluentResults;
using MediatR;
using MovieStore.Api.DTOs;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.CustomerHandlers
{
    public static class FindCustomers
    {
        public record Query(string value, PageDTO page) : IRequest<Result<PagedList<PromotedCustomerDTO>>>;
        public class FindCustomerHandler : IRequestHandler<Query, Result<PagedList<PromotedCustomerDTO>>>
        {
            private readonly ICustomerRepository _repository;

            public FindCustomerHandler(ICustomerRepository repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public async Task<Result<PagedList<PromotedCustomerDTO>>> Handle(Query request,
            CancellationToken cancellationToken)
            {
                PagedList<Customer> customers = await _repository.FindByValue(request.value, request.page);
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
