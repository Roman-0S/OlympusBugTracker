using OlympusBugTracker.Data;
using System.ComponentModel.DataAnnotations;

namespace OlympusBugTracker.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public Guid? ImageId { get; set; }

        public virtual FileUpload? Image { get; set; }

        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();

        public ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();

        public ICollection<Invite> Invites { get; set; } = new HashSet<Invite>();

    }
}
