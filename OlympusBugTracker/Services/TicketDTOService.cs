using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;

namespace OlympusBugTracker.Services
{
    public class TicketDTOService(ITicketRepository repository) : ITicketDTOService
    {
        public async Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId)
        {
            IEnumerable<Ticket> tickets = await repository.GetAllTicketsAsync(companyId);

            return tickets.Select(t => t.ToDTO());
        }
    }
}
