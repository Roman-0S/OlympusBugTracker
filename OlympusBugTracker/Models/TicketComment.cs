using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Data;
using System.ComponentModel.DataAnnotations;

namespace OlympusBugTracker.Models
{
    public class TicketComment
    {
        private DateTimeOffset _created;

        public int Id { get; set; }

        [Required]
        public string? Content { get; set; }

        public DateTimeOffset Created
        {
            get => _created;

            set => _created = value.ToUniversalTime();
        }

        public int TicketId { get; set; }

        public virtual Ticket? Ticket { get; set; }

        [Required]
        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }

    }

    public static class TicketCommentExtensions
    {
        public static TicketCommentDTO ToDTO(this TicketComment ticketComment)
        {
            TicketCommentDTO dto = new()
            {
                Id = ticketComment.Id,
                Content = ticketComment.Content,
                Created = ticketComment.Created,
                TicketId = ticketComment.TicketId,
                UserId = ticketComment.UserId,
            };

            if (ticketComment.User is not null)
            {
                dto.User = ticketComment.User.ToDTO();
            }

            return dto;
        }
    }

}
