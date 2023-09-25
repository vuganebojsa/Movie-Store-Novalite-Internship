using MovieStore.Core.Enums;

namespace MovieStore.Core.Model
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public LicensingType LicensingType { get; set; }
        public bool IsForRent() => LicensingType == Enums.LicensingType.TwoDay;
    }
}
