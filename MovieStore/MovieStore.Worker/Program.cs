using Microsoft.EntityFrameworkCore;
using MovieStore.Core.Model;
using MovieStore.Infrastructure;
using MovieStore.Infrastructure.Interfaces;
using MovieStore.Infrastructure.Repositories;
using MovieStore.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();

        // Retrieve the connection string from the configuration
        IConfiguration configuration = hostContext.Configuration;
        string connectionString = "Server=VUGA;Database=MovieStore;Trusted_Connection=True;TrustServerCertificate=True";

        services.AddDbContext<MovieStoreContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IPurchasedMovieRepository, PurchasedMovieRepository>();
        services.AddScoped<IRepositoryBase<Customer>, CustomerRepository>();
        services.AddScoped<IRepositoryBase<Movie>, MovieRepository>();


    })
    .Build();

await host.RunAsync();
