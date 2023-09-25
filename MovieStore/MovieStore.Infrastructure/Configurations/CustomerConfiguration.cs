using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Core.Model;

namespace MovieStore.Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(customer => customer.Id);
            builder.Property(customer => customer.Name).HasMaxLength(255);
            builder.OwnsOne(customer => customer.Email).Property(email => email.EmailAddress).HasColumnName("Email");
            builder.OwnsOne(customer => customer.Spent).Property(ammount => ammount.Amount).HasColumnName("Spent");
            builder.OwnsOne(customer => customer.Expiration).Property(expiration => expiration.Date).HasColumnName("Expiration");
        }
    }
}
