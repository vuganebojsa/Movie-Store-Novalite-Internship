using FluentResults;
using MediatR;
using MovieStore.Api.Extensions;
using MovieStore.Core.Messages;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.Handlers
{
    public static class AddCustomer
    {
        public record Command(Core.DTOs.CustomerDTO customer) : IRequest<Result<Core.Model.Customer>>;

        public class AddCustomerHandler : IRequestHandler<Command, Result<Core.Model.Customer>>
        {
            private readonly IRepositoryBase<Core.Model.Customer> _repository;
            public AddCustomerHandler(IRepositoryBase<Customer> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public async Task<Result<Core.Model.Customer>> Handle(Command request,
            CancellationToken cancellationToken)
            {

                var existingCustomer = await _repository.FindByCondition(customer => customer.Email.EmailAddress == request.customer.Email);
                if (existingCustomer.Any())
                {
                    return ResultExtensions.FailBadRequest<Core.Model.Customer>(ErrorMessages.AlreadyExists("Customer", "Email"));

                }
                var customer = await SaveCustomer(request);
                return Result.Ok(customer);

            }
            private async Task<Core.Model.Customer> SaveCustomer(Command request)
            {
                var email = Email.Create(request.customer.Email);
                var customer = Core.Model.Customer.Create(request.customer.Name, email.Value);

                await _repository.Create(customer);
                await _repository.SaveChanges();
                return customer;
            }
        }
    }
}
