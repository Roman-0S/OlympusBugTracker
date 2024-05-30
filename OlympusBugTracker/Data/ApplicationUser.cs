using Microsoft.AspNetCore.Identity;
using OlympusBugTracker.Client;
using OlympusBugTracker.Helpers;
using OlympusBugTracker.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OlympusBugTracker.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} at most {1} characters long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} at most {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [NotMapped]
        public string? FullName => $"{FirstName} {LastName}";

        public Guid? ImageId { get; set; }

        public virtual FileUpload? Image { get; set; }

        public int CompanyId { get; set; }

        public virtual Company? Company { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();

    }

    public static class ApplicationUserExtensions
    {
        public static UserInfo ToDTO(this ApplicationUser user)
        {
            UserInfo dto = new()
            {
                UserId = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                ProfilePictureUrl = user.ImageId.HasValue ? $"api/uploads/{user.ImageId}" : UploadHelper.DefaultProfilePicture,
            };            

            return dto;
        }
    }

}
