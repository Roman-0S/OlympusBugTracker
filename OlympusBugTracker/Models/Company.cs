using OlympusBugTracker.Client;
using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Data;
using OlympusBugTracker.Helpers;
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

    public static class CompanyExtensions
    {
        public static CompanyDTO ToDTO(this Company company)
        {
            CompanyDTO dto = new()
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                ImageURL = company.ImageId.HasValue ? $"api/uploads/{company.ImageId}" : UploadHelper.DefaultCompanyImage
            };

            foreach (Project project in company.Projects)
            {
                dto.Projects.Add(project.ToDTO());
            }

            foreach (ApplicationUser user in company.Users)
            {
                dto.Users.Add(user.ToDTO());
            }

            foreach (Invite invite in company.Invites)
            {
                dto.Invites.Add(invite.ToDTO());
            }

            return dto;
        }
    }

}
