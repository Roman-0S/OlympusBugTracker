using OlympusBugTracker.Client.Helpers;
using System.ComponentModel.DataAnnotations;

namespace OlympusBugTracker.Client.Models
{
    public class CompanyDTO
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string ImageURL { get; set; } = ImageHelper.DefaultCompanyImage;

        public ICollection<ProjectDTO> Projects { get; set; } = new HashSet<ProjectDTO>();

        public ICollection<UserDTO> Users { get; set; } = new HashSet<UserDTO>();

        public ICollection<InviteDTO> Invites { get; set; } = new HashSet<InviteDTO>();

    }
}
