using FluentResults;
using MediatR;
using Microsoft.Extensions.Options;
using MovieStore.Api.DTOs;
using MovieStore.Api.Extensions;
using MovieStore.Api.Options;
using MovieStore.Core.Messages;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Api.CustomerHandlers
{
    public static class PromoteCustomer
    {
        public record Query(Guid id) : IRequest<Result<PromotedCustomerDTO>>;

        public class PromoteCustomerHandler : IRequestHandler<Query, Result<PromotedCustomerDTO>>
        {
            private readonly ICustomerRepository _repository;
            private readonly CustomerPromotionOptions _promotionOptions;
            public PromoteCustomerHandler(ICustomerRepository repository, IOptions<CustomerPromotionOptions> promotionOptions)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _promotionOptions = promotionOptions.Value;
            }
            public async Task<Result<PromotedCustomerDTO>> Handle(Query request,
            CancellationToken cancellationToken)
            {
                var customer = await _repository.FindById(request.id);
                if (customer is null)
                {
                    return ResultExtensions.FailNotFound<PromotedCustomerDTO>(ErrorMessages.NotFound("Customer", "Id"));
                }
                if (customer.Spent < _promotionOptions.MinimumMoneySpent)
                {
                    return ResultExtensions.FailBadRequest<PromotedCustomerDTO>(ErrorMessages.CannotPromoteReasonMoney(_promotionOptions.MinimumMoneySpent));
                }
                if (customer.Status == Core.Enums.Status.Advanced
                    && customer.Expiration >= DateTime.Now)
                {
                    return ResultExtensions.FailBadRequest<PromotedCustomerDTO>(ErrorMessages.CannotPromoteReasonInvalidStatusDate());
                }

                var totalMoviesBought = await _repository.GetTotalMoviesBought(customer.Id, _promotionOptions.Days);
                if (totalMoviesBought < _promotionOptions.MinimumNumberOfMoviesBought)
                {
                    return ResultExtensions.FailBadRequest<PromotedCustomerDTO>(ErrorMessages.CannotPromoteReasonMoviesBought(totalMoviesBought, _promotionOptions.MinimumNumberOfMoviesBought));
                }
                var promotedCustomer = await PromoteCustomer(customer);
                return promotedCustomer.IsSuccess ? Result.Ok(promotedCustomer.Value) : Result.Fail(promotedCustomer.Errors.ElementAt(0).Message);
            }

            private async Task<Result<PromotedCustomerDTO>> PromoteCustomer(Core.Model.Customer customer)
            {
                var upgraded = customer.UpgradeStatus();
                if (upgraded.IsFailed) return Result.Fail(upgraded.Errors.ElementAt(0).Message);
                await _repository.SaveChanges();
                var promotedCustomer = new PromotedCustomerDTO { Email = customer.Email.EmailAddress, Id = customer.Id, MoneySpent = customer.Spent.Amount, Name = customer.Name };
                return Result.Ok(promotedCustomer);
            }
        }
    }
}
