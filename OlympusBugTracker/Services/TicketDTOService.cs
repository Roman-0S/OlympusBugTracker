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

            if (comment.User?.CompanyId == companyId)
            {
                await repository.AddCommentAsync(comment, companyId);
            }


        }

        #endregion

    }
}
