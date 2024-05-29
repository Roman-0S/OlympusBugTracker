using System.ComponentModel.DataAnnotations;
using static OlympusBugTracker.Client.Models.Enums;

namespace OlympusBugTracker.Client.Models
{
    public class TicketDTO
    {
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;

        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        public DateTimeOffset Created
        {
            get => _created;

            set => _created = value.ToUniversalTime();
        }

        public DateTimeOffset? Updated
        {
            get => _updated;

            set => _updated = value?.ToUniversalTime();
        }

        public bool Archived { get; set; }

        public bool ArchivedByProject { get; set; }

        public TicketPriority Priority { get; set; }

        public TicketType Type { get; set; }

        public TicketStatus Status { get; set; }

        public int ProjectId { get; set; }

        public ProjectDTO? Project { get; set; }

        [Required]
        public string? SubmitterUserId { get; set; }

        public UserInfo? SubmitterUser { get; set; }

        public string? DeveloperUserId { get; set; }

        public UserInfo? DeveloperUser { get; set; }

        public ICollection<TicketCommentDTO> TicketComments { get; set; } = new HashSet<TicketCommentDTO>();

        public ICollection<TicketAttachmentDTO> Attachments { get; set; } = new HashSet<TicketAttachmentDTO>();

        // History?

    }
}
