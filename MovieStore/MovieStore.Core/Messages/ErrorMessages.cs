namespace MovieStore.Core.Messages
{
    public static class ErrorMessages
    {
        public static string NotFound(string type, string attribute) => String.Format("The {0} with the given {1} doesn't exist.", type, attribute);
        public static string NotFoundCustomerId() => "The Customer with the given Id doesn't exist.";
        public static string NotFoundMovieId() => "The Movie with the given Id doesn't exist.";
        public static string MovieNotRentable() => "The movie is currently not purchasable/rentable.";
        public static string InvalidPrice() => "The price isn't valid.";
        public static string InvalidCustomerReference() => "Invalid Customer reference.";
        public static string EmailNotInCorrectFormat() => "The email is not in the correct format (example@mail.com).";
        public static string DecimalNumberAmount() => "The amount should not have more than 2 decimals.";
        public static string DecimalValueAmount() => "The amount should be above or equal to zero.";
        public static string CannotPromoteReasonMoney(decimal amount) => String.Format("The customer cannot be promoted beacause he didn't spend the minimum amount of money {0}.", amount);
        public static string CannotPromoteReasonInvalidStatusDate() => "The customer cannot be promoted. Invalid status or date.";
        public static string CannotPromoteReasonMoviesBought(int totalBought, int minimalMoviesToBuy) => String.Format("The customer cannot be promoted. The total number of movies bought: {0} is less that the minimum requirement: {1}.", totalBought, minimalMoviesToBuy);
        public static string CannotPromoteReasonAlreadyAdvanced() => "The customer cannot be promoted. He is already an advanced customer.";
        public static string AlreadyExists(string type, string attribute) => String.Format("The {0} with the given {1} already exists.", type, attribute);
    }
}
