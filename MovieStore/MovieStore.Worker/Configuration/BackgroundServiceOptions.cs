namespace MovieStore.Worker.Configuration
{
    public class BackgroundServiceOptions
    {
        public const string BackgroundService = "BackgroundService";
        public int SendEmailInterval { get; set; } = 5;
    }
}
