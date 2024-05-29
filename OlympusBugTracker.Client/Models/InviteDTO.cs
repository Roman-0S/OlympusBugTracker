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

        public string? InviteeEmail { get; set; }

        public string? InviteeFirstName { get; set; }

        public string? InviteeLastName { get; set; }

        public string? InviteMessage { get; set; }

        public bool IsValid { get; set; }

        public int ProjectId { get; set; }

        public ProjectDTO? Project { get; set; }

        public string? InvitorId { get; set; }

        public UserInfo? Invitor { get; set; }

        public string? InviteeId { get; set; }

        public UserInfo? Invitee { get; set; }

    }
}
