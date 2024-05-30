using System.ComponentModel.DataAnnotations;

namespace OlympusBugTracker.Client.Models
{
    public class InviteDTO
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

        [Required]
        public string? InviteeEmail { get; set; }

        [Required]
        public string? InviteeFirstName { get; set; }

        [Required]
        public string? InviteeLastName { get; set; }

        public string? InviteMessage { get; set; }

        public bool IsValid { get; set; }

        public int ProjectId { get; set; }

        public ProjectDTO? Project { get; set; }

        [Required]
        public string? InvitorId { get; set; }

        public UserDTO? Invitor { get; set; }

        public string? InviteeId { get; set; }

        public UserDTO? Invitee { get; set; }

    }
}
