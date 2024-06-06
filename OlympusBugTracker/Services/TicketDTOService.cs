using OlympusBugTracker.Client.Helpers;
using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using OlympusBugTracker.Data;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;

namespace OlympusBugTracker.Services
{
    public class TicketDTOService(ITicketRepository repository) : ITicketDTOService
    {

        #region Tickets

        public async Task<TicketDTO> AddTicketAsync(TicketDTO ticketDTO, int companyId)
        {
            Ticket ticket = new()
            {
                Title = ticketDTO.Title,
                Description = ticketDTO.Description,
                Priority = ticketDTO.Priority,
                Type = ticketDTO.Type,
                Status = ticketDTO.Status,
                SubmitterUserId = ticketDTO.SubmitterUserId,
                ProjectId = ticketDTO.ProjectId
            };

            //ticket.Project = await projectRepository.GetProjectByIdAsync(ticket.ProjectId, companyId);

            ticket = await repository.AddTicketAsync(ticket, companyId);

            return ticket.ToDTO();
        }

        public async Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId)
        {
            IEnumerable<Ticket> tickets = await repository.GetAllTicketsAsync(companyId);

            return tickets.Select(t => t.ToDTO());
        }

        public async Task<IEnumerable<TicketDTO>> GetArchivedTicketsAsync(int companyId)
        {
            IEnumerable<Ticket> tickets = await repository.GetArchivedTicketsAsync(companyId);

            return tickets.Select(t => t.ToDTO());
        }

        public async Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId)
        {
            Ticket? ticket = await repository.GetTicketByIdAsync(ticketId, companyId);

            return ticket?.ToDTO();
        }

        public async Task UpdateTicketAsync(TicketDTO ticketDTO, int companyId, string userId)
        {
            Ticket? ticket = await repository.GetTicketByIdAsync(ticketDTO.Id, companyId);

            if (ticket is not null)
            {
                ticket.Title = ticketDTO.Title;
                ticket.Description = ticketDTO.Description;
                ticket.Priority = ticketDTO.Priority;
                ticket.Type = ticketDTO.Type;
                ticket.Status = ticketDTO.Status;
                ticket.SubmitterUserId = ticketDTO.SubmitterUserId;
                ticket.DeveloperUserId = ticketDTO.DeveloperUserId;

                ticket.DeveloperUser = null;

                await repository.UpdateTicketAsync(ticket, companyId, userId);
            }
        }

        public async Task ArchiveTicketAsync(int ticketId, int companyId)
        {
            await repository.ArchiveTicketAsync(ticketId, companyId);
        }

        public async Task RestoreTicketAsync(int ticketId, int companyId)
        {
            await repository.RestoreTicketAsync(ticketId, companyId);
        }

        #endregion

        #region Ticket Comments

        public async Task AddCommentAsync(TicketCommentDTO commentDTO, int companyId)
        {
            TicketComment comment = new()
            {
                Content = commentDTO.Content,
                TicketId = commentDTO.TicketId,
                UserId = commentDTO.UserId,
            };


            await repository.AddCommentAsync(comment, companyId);

        }

        public async Task<IEnumerable<TicketCommentDTO>> GetTicketCommentsAsync(int ticketId, int companyId)
        {
            IEnumerable<TicketComment> comments = await repository.GetTicketCommentsAsync(ticketId, companyId);

            return comments.Select(c => c.ToDTO());
        }

        public async Task<TicketCommentDTO?> GetCommentByIdAsync(int commentId, int companyId)
        {
            TicketComment? comment = await repository.GetCommentByIdAsync(commentId, companyId);

            return comment?.ToDTO();
        }

        public async Task DeleteCommentAsync(int commentId, int companyId)
        {
            await repository.DeleteCommentAsync(commentId, companyId);
        }

        public async Task UpdateCommentAsync(TicketCommentDTO commentDTO, int companyId, string userId)
        {
            TicketComment? comment = await repository.GetCommentByIdAsync(commentDTO.Id, companyId);

            if (comment is not null)
            {
                comment.Content = commentDTO.Content;

                await repository.UpdateCommentAsync(comment, companyId, userId);
            }
        }

        #endregion

        #region Ticket Attachments

        public async Task<TicketAttachmentDTO> AddTicketAttachment(TicketAttachmentDTO attachmentDTO, byte[] uploadData, string contentType, int companyId)
        {
            FileUpload file = new()
            {
                Type = contentType,
                Data = uploadData,
            };

            TicketAttachment attachment = new()
            {
                TicketId = attachmentDTO.TicketId,
                Description = attachmentDTO.Description,
                FileName = attachmentDTO.FileName,
                Upload = file,
                Created = DateTimeOffset.Now,
                UserId = attachmentDTO.UserId,
            };

            attachment = await repository.AddTicketAttachment(attachment, companyId);

            return attachment.ToDTO();
        }

        public async Task DeleteTicketAttachment(int attachmentId, int companyId)
        {
            await repository.DeleteTicketAttachment(attachmentId, companyId);
        }

        #endregion

    }
}
