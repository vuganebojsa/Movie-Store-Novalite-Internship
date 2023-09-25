using Microsoft.EntityFrameworkCore;
using MovieStore.Core.Model;
using System.Reflection;

namespace MovieStore.Infrastructure
{
    public class MovieStoreContext : DbContext
    {
        public MovieStoreContext(DbContextOptions<MovieStoreContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<PurchasedMovie> PurchasedMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
