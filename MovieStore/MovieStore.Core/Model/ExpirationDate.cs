namespace MovieStore.Core.Model
{
    public record ExpirationDate
    {
        public DateTime? Date { get; set; }
        public static ExpirationDate Infinity => new(null as DateTime?);
        public ExpirationDate(DateTime? date) => Date = date;
        private ExpirationDate() { }
        public bool IsExpired => this != Infinity && this.Date!.Value < DateTime.Now;

        public static bool operator >=(ExpirationDate expirationDate, DateTime otherDate) => expirationDate.Date >= otherDate;
        public static bool operator <=(ExpirationDate expirationDate, DateTime otherDate) => expirationDate.Date <= otherDate;
    }
}
