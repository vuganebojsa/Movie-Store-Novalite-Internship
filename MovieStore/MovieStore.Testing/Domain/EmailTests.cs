using FluentAssertions;
using FluentResults;
using MovieStore.Core.Model;

namespace MovieStore.Testing.Domain
{
    public class EmailTests
    {

        [Theory]
        [InlineData("nebojsa@gmail.com")]
        [InlineData("nebojsa@mail.rs")]
        [InlineData("kapa@email.es")]
        public void Create_ShouldReturnResultOk_WithValidEmail(string email)
        {
            var newEmail = Email.Create(email);
            newEmail.Should().BeOfType<Result<Email>>();
            newEmail.IsSuccess.Should().BeTrue();
            newEmail.Value.EmailAddress.Should().Be(email);

        }

        [Theory]
        [InlineData("nebojsagmail.com")]
        [InlineData("nebojsa@mailrs")]
        [InlineData("")]
        [InlineData("aa@@@@.rs")]
        public void Create_ShouldReturnResultFail_WithInvalidEmail(string email)
        {
            var newEmail = Email.Create(email);
            newEmail.Should().BeOfType<Result<Email>>();
            newEmail.IsFailed.Should().BeTrue();
            newEmail.Errors.Should().NotBeEmpty();
            newEmail.Errors.ElementAt(0).Message.Should().BeEquivalentTo("The email is not in the correct format (example@mail.com).");

        }
    }
}
