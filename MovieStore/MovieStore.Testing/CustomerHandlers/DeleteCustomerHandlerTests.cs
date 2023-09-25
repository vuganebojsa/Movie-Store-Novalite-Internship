using FakeItEasy;
using FluentAssertions;
using MovieStore.Api.Handlers;
using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;

namespace MovieStore.Testing.CustomerHandlers
{
    public class DeleteCustomerHandlerTests
    {
        private readonly DeleteCustomer.DeleteCustomerHandler _customerHandler;
        private readonly IRepositoryBase<Customer> _repository;

        public DeleteCustomerHandlerTests()
        {
            _repository = A.Fake<IRepositoryBase<Customer>>();
            _customerHandler = new DeleteCustomer.DeleteCustomerHandler(_repository);
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsResultOk()
        {
            var customerId = Guid.NewGuid();
            var customer = Customer.Create("nebojsa", Email.Create("nebojsa@gmail.com").Value);

            A.CallTo(() => _repository.FindById(customerId)).Returns(customer);
            var command = new DeleteCustomer.Command(customerId);

            var result = await _customerHandler.Handle(command, CancellationToken.None);

            A.CallTo(() => _repository.Delete(A<Customer>.Ignored)).MustHaveHappenedOnceExactly();

            result.IsSuccess.Should().BeTrue();

        }

        [Fact]
        public async Task Handle_InvalidId_ReturnsFailFromResult()
        {
            var customerId = Guid.NewGuid();

            A.CallTo(() => _repository.FindById(customerId)).Returns(Task.FromResult<Customer>(null));
            var command = new DeleteCustomer.Command(customerId);

            var result = await _customerHandler.Handle(command, CancellationToken.None);

            result.IsFailed.Should().BeTrue();
            A.CallTo(() => _repository.Delete(A<Customer>.Ignored)).MustNotHaveHappened();

            result.Errors.ElementAt(0).Message.Should().BeEquivalentTo("The customer with the given id doesn't exist.");
        }
    }
}
