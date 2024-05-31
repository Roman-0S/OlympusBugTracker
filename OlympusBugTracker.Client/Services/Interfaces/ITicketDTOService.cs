using OlympusBugTracker.Client.Models;

namespace OlympusBugTracker.Client.Services.Interfaces
{
    public interface ITicketDTOService
    {
        Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId);

    }
}
