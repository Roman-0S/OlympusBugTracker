using System.ComponentModel.DataAnnotations;
using static OlympusBugTracker.Client.Models.Enums;

namespace OlympusBugTracker.Client.Models
{
    public class ProjectDTO
    {
        private DateTimeOffset _startDate;
        private DateTimeOffset _created;
        private DateTimeOffset? _endDate;

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
        
        public DateTimeOffset? EndDate
        {
            get => _endDate;

            set => _endDate = value?.ToUniversalTime();
        }

        public ProjectPriority Priority { get; set; }

        public bool Archived { get; set; }

        public ICollection<UserDTO> Users { get; set; } = new HashSet<UserDTO>();

        public ICollection<TicketDTO> Tickets { get; set; } = new HashSet<TicketDTO>();

    }
}
