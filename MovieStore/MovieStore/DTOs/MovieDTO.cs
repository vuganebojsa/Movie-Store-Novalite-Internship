using MovieStore.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace MovieStore.Core.DTOs
{
    public class MovieDTO
    {
        [Required]
        [MaxLength(255, ErrorMessage = "Maximum length of movie name exceded. Please keep it below 255 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public LicensingType LicensingType { get; set; } = LicensingType.TwoDay;
    }
}
