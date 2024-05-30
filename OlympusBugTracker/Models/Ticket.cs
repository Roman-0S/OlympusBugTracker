using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Data;
using System.ComponentModel.DataAnnotations;
using static OlympusBugTracker.Client.Models.Enums;

namespace OlympusBugTracker.Models
{
    public class Ticket
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

        public virtual Project? Project { get; set; }

        [Required]
        public string? SubmitterUserId { get; set; }

        public virtual ApplicationUser? SubmitterUser { get; set; }

        public string? DeveloperUserId { get; set; }

        public virtual ApplicationUser? DeveloperUser { get; set; }

        public ICollection<TicketComment> TicketComments { get; set; } = new HashSet<TicketComment>();

        public ICollection<TicketAttachment> TicketAttachments { get; set; } = new HashSet<TicketAttachment>();

    }

    public static class TicketExtensions
    {
        public static TicketDTO ToDTO(this Ticket ticket)
        {
            TicketDTO dto = new()
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Created = ticket.Created,
                Updated = ticket.Updated,
                Archived = ticket.Archived,
                ArchivedByProject = ticket.ArchivedByProject,
                Priority = ticket.Priority,
                Type = ticket.Type,
                Status = ticket.Status,
                ProjectId = ticket.ProjectId,
                SubmitterUserId = ticket.SubmitterUserId,
                DeveloperUserId = ticket.DeveloperUserId,
            };

            if (ticket.Project is not null)
            {
                ticket.Project.Tickets = [];

                dto.Project = ticket.Project.ToDTO();
            }

            if (ticket.SubmitterUser is not null)
            {
                dto.SubmitterUser = ticket.SubmitterUser.ToDTO();
            }

            if (ticket.DeveloperUser is not null)
            {
                dto.DeveloperUser = ticket.DeveloperUser.ToDTO();
            }

            foreach (TicketComment comment in ticket.TicketComments)
            {
                dto.TicketComments.Add(comment.ToDTO());
            }

            foreach (TicketAttachment attachment in ticket.TicketAttachments)
            {
                dto.Attachments.Add(attachment.ToDTO());
            }

            return dto;
        }
    }

}
