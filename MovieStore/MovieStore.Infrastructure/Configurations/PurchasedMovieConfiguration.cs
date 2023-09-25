using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Core.Model;

namespace MovieStore.Infrastructure.Configurations
{
    public class PurchasedMovieConfiguration : IEntityTypeConfiguration<PurchasedMovie>
    {
        public void Configure(EntityTypeBuilder<PurchasedMovie> builder)
        {
            builder.HasKey(purchasedMovie => purchasedMovie.Id);
            builder
               .HasOne(purchasedMovie => purchasedMovie.Movie)
            .WithMany();
            builder
                .HasOne(purchasedMovie => purchasedMovie.Customer)
                .WithMany(customer => customer.PurchasedMovies);
            builder.OwnsOne(purchasedMovie => purchasedMovie.Price).Property(ammount => ammount.Amount).HasColumnName("Amount");
            builder.OwnsOne(purchasedMovie => purchasedMovie.ExpirationDate).Property(expiration => expiration.Date).HasColumnName("Expiration");
        }
    }
}
