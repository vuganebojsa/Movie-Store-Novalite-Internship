using MovieStore.Core.Model;

namespace MovieStore.Testing.Domain
{
    public class ExpirationDateTests
    {
        [Fact]
        public void IsExpired_ShouldReturnTrueOnOldTime()
        {
            var date = new ExpirationDate(DateTime.Now.AddDays(-3));

            var isExpired = date.IsExpired;
            Assert.True(isExpired);
        }

        [Fact]
        public void IsExpired_ShouldReturnFalseOnNewTime()
        {
            var date = new ExpirationDate(DateTime.Now.AddDays(3));

            var isExpired = date.IsExpired;
            Assert.False(isExpired);
        }
    }
}
