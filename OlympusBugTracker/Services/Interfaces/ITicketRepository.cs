using OlympusBugTracker.Models;

namespace OlympusBugTracker.Services.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync(int companyId);
    }
}
