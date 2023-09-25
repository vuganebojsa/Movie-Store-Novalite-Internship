using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStore.Api.Filters;
using MovieStore.Api.LoggingHandlers;
using MovieStore.Api.Options;
using MovieStore.Api.Swagger;
using MovieStore.Core.Model;
using MovieStore.Infrastructure;
using MovieStore.Infrastructure.Interfaces;
using MovieStore.Infrastructure.Repositories;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();
    loggerConfiguration.ReadFrom.Configuration(configuration);

});
builder.Services.AddDbContext<MovieStoreContext>(

    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("."), b => b.MigrationsAssembly("MovieStore.Infrastructure"));
    }, ServiceLifetime.Singleton
);

builder.Services.AddMediatR(
config => config.RegisterServicesFromAssembly(typeof(Program).Assembly)
);
builder.Services.Configure<CustomerPromotionOptions>(builder.Configuration.GetSection("CustomerPromotionRequirements"));
builder.Services.Configure<MoviePriceOptions>(builder.Configuration.GetSection("MoviePrice"));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

builder.Services.AddControllers(
    options =>
    { options.Filters.Add(typeof(ModelStateValidationFilter)); }

    );

builder.Services.AddScoped<IRepositoryBase<Customer>, CustomerRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IPurchasedMovieRepository, PurchasedMovieRepository>();
builder.Services.AddScoped<IRepositoryBase<Movie>, MovieRepository>();
builder.Services.AddScoped<IRepositoryBase<PurchasedMovie>, PurchasedMovieRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument
    (
    options =>
    {
        options.SchemaNameGenerator = new CustomSwaggerSchemaNameGenerator();
        options.Title = "MovieStore";

    }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(options =>
    {
        options.Path = "/swagger/{documentName}/swagger.json";
    });

    app.UseSwaggerUi3(options =>
    {
        options.DocumentPath = "/swagger/{documentName}/swagger.json";
    });

    // Redirect root URL to Swagger
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
            return;
        }

        await next();
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<MovieStoreContext>();

    dbContext.Database.Migrate();

}
app.MapControllers();
try
{
    Log.Information("Starting MovieStore");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated");
}
finally
{
    Log.CloseAndFlush();
}
