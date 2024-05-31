using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;

namespace OlympusBugTracker.Services
{
    public class TicketDTOService(ITicketRepository repository, IProjectRepository projectRepository) : ITicketDTOService
    {
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

            ticket.Project = await projectRepository.GetProjectByIdAsync(ticket.ProjectId, companyId);

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

        public async Task ArchiveTicketAsync(int ticketId, int companyId)
        {
            await repository.ArchiveTicketAsync(ticketId, companyId);
        }

        public async Task RestoreTicketAsync(int ticketId, int companyId)
        {
            await repository.RestoreTicketAsync(ticketId, companyId);
        }

    }
}
