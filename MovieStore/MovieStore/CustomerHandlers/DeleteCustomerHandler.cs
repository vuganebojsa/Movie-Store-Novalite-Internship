using FluentResults;
using MediatR;
using MovieStore.Api.Extensions;
using MovieStore.Core.Messages;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.Handlers
{
    public static class DeleteCustomer
    {
        public record Command(Guid id) : IRequest<Result>;

        public class DeleteCustomerHandler : IRequestHandler<Command, Result>
        {
            private readonly IRepositoryBase<Customer> _repository;

            public DeleteCustomerHandler(IRepositoryBase<Customer> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public async Task<Result> Handle(Command request,
            CancellationToken cancellationToken)
            {
                var customer = await _repository.FindById(request.id);
                if (customer is null)
                {
                    return ResultExtensions.FailNotFound(ErrorMessages.NotFound("Customer", "Id"));
                }
                _repository.Delete(customer);
                await _repository.SaveChanges();
                return Result.Ok();

            }
        }
    }
}
