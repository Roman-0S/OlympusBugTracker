﻿using OlympusBugTracker.Client.Models;

namespace OlympusBugTracker.Client.Services.Interfaces
{
    public interface ITicketDTOService
    {
        Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId);

        Task<IEnumerable<TicketDTO>> GetArchivedTicketsAsync(int companyId);

        Task<TicketDTO> AddTicketAsync(TicketDTO ticketDTO, int companyId);

        Task ArchiveTicketAsync(int ticketId, int companyId);

        Task RestoreTicketAsync(int ticketId, int companyId);


    }
}
