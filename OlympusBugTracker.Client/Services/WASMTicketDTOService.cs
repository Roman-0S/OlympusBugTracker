using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace OlympusBugTracker.Client.Services
{
    public class WASMTicketDTOService : ITicketDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMTicketDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId)
        {
            IEnumerable<TicketDTO> tickets = await _httpClient.GetFromJsonAsync<IEnumerable<TicketDTO>>("api/tickets") ?? [];

            return tickets;
        }

        public async Task<IEnumerable<TicketDTO>> GetArchivedTicketsAsync(int companyId)
        {
            IEnumerable<TicketDTO> tickets = await _httpClient.GetFromJsonAsync<IEnumerable<TicketDTO>>("api/tickets/archived") ?? [];

            return tickets;
        }

        public async Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId)
        {
            TicketDTO? ticket = await _httpClient.GetFromJsonAsync<TicketDTO>($"api/tickets/ticket/{ticketId}");

            return ticket;
        }

        public async Task<TicketDTO> AddTicketAsync(TicketDTO ticketDTO, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<TicketDTO>("api/tickets", ticketDTO);
            response.EnsureSuccessStatusCode();

            TicketDTO? ticket = await response.Content.ReadFromJsonAsync<TicketDTO>();
            return ticket!;
        }

        public async Task UpdateTicketAsync(TicketDTO ticketDTO, int companyId, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<TicketDTO>($"api/tickets/{ticketDTO.Id}", ticketDTO);
            response.EnsureSuccessStatusCode();
        }

        public async Task ArchiveTicketAsync(int ticketId, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<string>($"api/tickets/archive/{ticketId}", "");
            response.EnsureSuccessStatusCode();
        }

        public async Task RestoreTicketAsync(int ticketId, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<string>($"api/tickets/restore/{ticketId}", "");
            response.EnsureSuccessStatusCode();
        }

    }
}
