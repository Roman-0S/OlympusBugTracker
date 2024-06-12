using OlympusBugTracker.Client.Models;

namespace OlympusBugTracker.Client.Services.Interfaces
{
    public interface ITicketDTOService
    {
        #region Tickets

        Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId);

        Task<IEnumerable<TicketDTO>> GetArchivedTicketsAsync(int companyId);

        Task<IEnumerable<TicketDTO>> GetUserTicketsAsync(string userId, int companyId);

        Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId);

        Task<TicketDTO> AddTicketAsync(TicketDTO ticketDTO, int companyId);

        Task UpdateTicketAsync(TicketDTO ticketDTO, int companyId, string userId);

        Task ArchiveTicketAsync(int ticketId, int companyId);

        Task RestoreTicketAsync(int ticketId, int companyId);

        #endregion

        #region Ticket Comments

        Task AddCommentAsync(TicketCommentDTO commentDTO, int companyId);

        Task<IEnumerable<TicketCommentDTO>> GetTicketCommentsAsync(int ticketId, int companyId);

        Task<TicketCommentDTO?> GetCommentByIdAsync(int commentId, int companyId);

        Task DeleteCommentAsync(int commentId, int companyId);

        Task UpdateCommentAsync(TicketCommentDTO commentDTO, int companyId, string userId);

        #endregion

        #region Ticket Attachment

        Task<TicketAttachmentDTO> AddTicketAttachment(TicketAttachmentDTO attachmentDTO, byte[] uploadData, string contentType, int companyId);

        Task<TicketAttachmentDTO?> GetTicketAttachmentByIdAsync(int attachmentId, int companyId);

        Task DeleteTicketAttachment(int attachmentId, int companyId);

        #endregion

    }
}
