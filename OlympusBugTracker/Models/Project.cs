using OlympusBugTracker.Data;
using System.ComponentModel.DataAnnotations;
using static OlympusBugTracker.Client.Models.Enums;

namespace OlympusBugTracker.Models
{
    public class Project
    {
        private DateTimeOffset _startDate;
        private DateTimeOffset? _endDate;

        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public DateTimeOffset StartDate
        {
            get => _startDate;

            set => _startDate = value.ToUniversalTime();
        }

        public DateTimeOffset? EndDate
        {
            get => _endDate;

            set => _endDate = value?.ToUniversalTime();
        }

        public ProjectPriority Priority { get; set; }

        public bool Archived { get; set; }

        public int CompanyId { get; set; }

        public virtual Company? Company { get; set; }

        public ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }
}
