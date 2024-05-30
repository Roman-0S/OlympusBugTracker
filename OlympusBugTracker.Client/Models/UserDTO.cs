using OlympusBugTracker.Client.Helpers;
using System.ComponentModel.DataAnnotations;

namespace OlympusBugTracker.Client.Models
{
    public class UserDTO
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? FullName => $"{FirstName} {LastName}";

        public string ImageURL { get; set; } = ImageHelper.DefaultProfilePicture;

        [Required]
        public string? Email { get; set; }

    }
}
