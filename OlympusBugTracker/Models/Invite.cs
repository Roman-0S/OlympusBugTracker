using OlympusBugTracker.Client.Models;
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

    public static class InviteExtensions
    {
        public static InviteDTO ToDTO(this Invite invite)
        {
            InviteDTO dto = new()
            {
                Id = invite.Id,
                InviteDate = invite.InviteDate,
                JoinDate = invite.JoinDate,
                InviteeEmail = invite.InviteeEmail,
                InviteeFirstName = invite.InviteeFirstName,
                InviteeLastName = invite.InviteeLastName,
                InviteMessage = invite.InviteMessage,
                IsValid = invite.IsValid,
                ProjectId = invite.ProjectId,
                InvitorId = invite.InvitorId,
                InviteeId = invite.InviteeId,
            };

            if (invite.Project is not null)
            {
                dto.Project = invite.Project.ToDTO();
            }

            if (invite.Invitor is not null)
            {
                dto.Invitor = invite.Invitor.ToDTO();
            }

            if (invite.Invitee is not null)
            {
                dto.Invitee = invite.Invitee.ToDTO();
            }

            return dto;
        }
    }
}
