using System.ComponentModel.DataAnnotations;

namespace MovieStore.Core.DTOs
{
    public class CustomerDTO
    {
        [Required]
        [MaxLength(255, ErrorMessage = "The maximum name length is 255 characters. Please keep it below that.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "The Email is not in the correct format.")]
        public string Email { get; set; } = string.Empty;
    }
}
