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

        #endregion

    }
}
