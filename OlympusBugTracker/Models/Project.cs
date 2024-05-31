using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Data;
using System.ComponentModel.DataAnnotations;
using static OlympusBugTracker.Client.Models.Enums;

namespace OlympusBugTracker.Models
{
    public class Project
    {
        private DateTimeOffset _startDate;
        private DateTimeOffset _created;
        private DateTimeOffset _endDate;

        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public DateTimeOffset Created
        {
            get => _created;

            set => _created = value.ToUniversalTime();
        }

        public DateTimeOffset StartDate
        {
            get => _startDate;

            set => _startDate = value.ToUniversalTime();
        }

        public DateTimeOffset EndDate
        {
            get => _endDate;

            set => _endDate = value.ToUniversalTime();
        }

        public ProjectPriority Priority { get; set; }

        public bool Archived { get; set; }

        public int CompanyId { get; set; }

        public virtual Company? Company { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }

    public static class ProjectExtensions
    {
        public static ProjectDTO ToDTO(this Project project)
        {
            ProjectDTO dto = new()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Created = project.Created,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Priority = project.Priority,
                Archived = project.Archived,
                
            };

            foreach (ApplicationUser user in project.Users)
            {
                dto.Users.Add(user.ToDTO());
            }

            foreach (Ticket ticket in project.Tickets)
            {
                dto.Tickets.Add(ticket.ToDTO());
            }

            return dto;
        }
    }

}
