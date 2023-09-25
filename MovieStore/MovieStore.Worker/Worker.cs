using MovieStore.Core.Model;
using MovieStore.Infrastructure.Interfaces;
using MovieStore.Worker.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MovieStore.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var backgroundServiceOptions = new BackgroundServiceOptions();
            _configuration.GetSection(BackgroundServiceOptions.BackgroundService).Bind(backgroundServiceOptions);
            var minutes = backgroundServiceOptions.SendEmailInterval;
            var emailSendInterval = TimeSpan.FromMinutes(minutes);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using var scope = _serviceProvider.CreateScope();
                var purchasedMovies = await GetExpiredPurchasedMovies(scope);

                if (purchasedMovies != null) await SendEmailsToUsers(purchasedMovies);

                await Task.Delay(emailSendInterval, stoppingToken);
            }
        }

        private async Task SendEmailsToUsers(ICollection<PurchasedMovie> purchasedMovies)
        {
            var apiKey = Environment.GetEnvironmentVariable("APIKEYNOVALITESENDGRID");
            var client = new SendGridClient(apiKey);

            foreach (var purchasedMovie in purchasedMovies)
            {
                await SendEmail(client, purchasedMovie);
            }
        }

        private static async Task SendEmail(SendGridClient client, PurchasedMovie purchasedMovie)
        {
            var from = new EmailAddress("vuga.sv53.2020@uns.ac.rs", "Nebojsa Vuga");
            var to = new EmailAddress(purchasedMovie.Customer.Email.EmailAddress);
            var subject = "Your " + purchasedMovie.Movie.Name + " purchased has expired.";
            var plainContent = "Your " + purchasedMovie.Movie.Name + " movie rent has expired. Please return it to the store.";
            var content = "Your <strong>" + purchasedMovie.Movie.Name + "</strong> movie rent has expired. Please return it to the store.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainContent, content);
            await client.SendEmailAsync(msg);
        }

        private static async Task<ICollection<PurchasedMovie>> GetExpiredPurchasedMovies(IServiceScope scope)
        {
            var purchasedMovieRepository = scope.ServiceProvider.GetRequiredService<IPurchasedMovieRepository>();

            var purchasedMovies = await purchasedMovieRepository.GetExpiredPurchasedMovies();
            return purchasedMovies;
        }


    }
}