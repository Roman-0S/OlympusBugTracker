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

        #region Tickets

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

        #endregion

        #region Ticket Comments

        public async Task AddCommentAsync(TicketCommentDTO commentDTO, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/tickets/comments", commentDTO);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<TicketCommentDTO>> GetTicketCommentsAsync(int ticketId, int companyId)
        {
            IEnumerable<TicketCommentDTO> comments = await _httpClient.GetFromJsonAsync<IEnumerable<TicketCommentDTO>>($"api/tickets/{ticketId}/comments") ?? [];

            return comments;
        }

        public async Task<TicketCommentDTO?> GetCommentByIdAsync(int commentId, int companyId)
        {
            TicketCommentDTO? comment = await _httpClient.GetFromJsonAsync<TicketCommentDTO>($"api/tickets/comments/{commentId}");

            return comment;
        }

        public async Task DeleteCommentAsync(int commentId, int companyId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/tickets/{commentId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCommentAsync(TicketCommentDTO commentDTO, int companyId, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<TicketCommentDTO>($"api/tickets/comment/{commentDTO.Id}", commentDTO);
            response.EnsureSuccessStatusCode();
        }

        #endregion
    }
}
