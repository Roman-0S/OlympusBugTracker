using OlympusBugTracker.Models;

namespace OlympusBugTracker.Services.Interfaces
{
    public interface ITicketRepository
    {

        #region Tickets
        Task<IEnumerable<Ticket>> GetAllTicketsAsync(int companyId);

        Task<IEnumerable<Ticket>> GetArchivedTicketsAsync(int companyId);

        Task<Ticket?> GetTicketByIdAsync(int ticketId, int companyId);

        Task<Ticket> AddTicketAsync(Ticket ticket, int companyId);

        Task UpdateTicketAsync(Ticket ticket, int companyId, string userId);

        Task ArchiveTicketAsync(int ticketId, int companyId);

        Task RestoreTicketAsync(int ticketId, int companyId);

        #endregion

        #region Ticket Comments

        Task AddCommentAsync(TicketComment comment, int companyId);

        Task<IEnumerable<TicketComment>> GetTicketCommentsAsync(int ticketId, int companyId);

        Task<TicketComment?> GetCommentByIdAsync(int commentId, int companyId);

        Task DeleteCommentAsync(int commentId, int companyId);

        Task UpdateCommentAsync(TicketComment comment, int companyId, string userId);

        #endregion

        #region Ticket Attachments

        Task<TicketAttachment> AddTicketAttachment(TicketAttachment attachment, int companyId);

        Task DeleteTicketAttachment(int attachmentId, int companyId);

        #endregion

    }
}
