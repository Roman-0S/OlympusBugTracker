using OlympusBugTracker.Data;
using System.ComponentModel.DataAnnotations;

namespace OlympusBugTracker.Models
{
    public class Invite
    {
        private DateTimeOffset _inviteDate;
        private DateTimeOffset? _joinDate;

        public int Id { get; set; }

        public DateTimeOffset InviteDate
        {
            get => _inviteDate;

            set => _inviteDate = value.ToUniversalTime();
        }

        public DateTimeOffset? JoinDate
        {
            get => _joinDate;

            set => _joinDate = value?.ToUniversalTime();
        }

        public Guid CompanyToken { get; set; }

        [Required]
        public string? InviteeEmail { get; set; }

        [Required]
        public string? InviteeFirstName { get; set; }

        [Required]
        public string? InviteeLastName { get; set; }

        public string? InviteMessage { get; set; }

        public bool IsValid { get; set; }

        public int CompanyId { get; set; }

        public virtual Company? Company { get; set; }

        public int ProjectId { get; set; }

        public virtual Project? Project { get; set; }

        [Required]
        public string? InvitorId { get; set; }

        public virtual ApplicationUser? Invitor { get; set; }

        public string? InviteeId { get; set; }

        public virtual ApplicationUser? Invitee { get; set; }

    }
}
