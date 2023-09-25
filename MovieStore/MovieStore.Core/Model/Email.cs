using FluentResults;
using MovieStore.Core.Messages;
using System.Text.RegularExpressions;

namespace MovieStore.Core.Model
{
    public record Email
    {
        public string EmailAddress { get; init; }
        public Email(string email) => EmailAddress = email;
        private Email() { }
        public static Result<Email> Create(string email)
        {
            string regex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, regex) ?
                Result.Ok<Email>(new(email)) : Result.Fail(ErrorMessages.EmailNotInCorrectFormat());
        }

        public static bool operator ==(Email obj1, string obj2) => obj1.EmailAddress == obj2;
        public static bool operator !=(Email obj1, string obj2) => obj1.EmailAddress != obj2;
    }
}
