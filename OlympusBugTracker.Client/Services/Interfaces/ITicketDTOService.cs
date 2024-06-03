using OlympusBugTracker.Client.Models;

namespace OlympusBugTracker.Client.Services.Interfaces
{
    public interface ITicketDTOService
    {
        #region Tickets

        Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId);

        Task<IEnumerable<TicketDTO>> GetArchivedTicketsAsync(int companyId);

        Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId);

        Task<TicketDTO> AddTicketAsync(TicketDTO ticketDTO, int companyId);

        Task UpdateTicketAsync(TicketDTO ticketDTO, int companyId, string userId);

        Task ArchiveTicketAsync(int ticketId, int companyId);

        Task RestoreTicketAsync(int ticketId, int companyId);

        #endregion

        #region Ticket Comments

        Task AddCommentAsync(TicketCommentDTO commentDTO, int companyId);

        #endregion

    }
}
