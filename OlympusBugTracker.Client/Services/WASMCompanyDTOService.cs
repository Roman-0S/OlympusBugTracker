using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace OlympusBugTracker.Client.Services
{
    public class WASMCompanyDTOService : ICompanyDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMCompanyDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CompanyDTO?> GetCompanyByIdAsync(int companyId)
        {
            CompanyDTO? company = await _httpClient.GetFromJsonAsync<CompanyDTO>("api/companies");

            return company;
        }

        public async Task<IEnumerable<UserDTO>> GetCompanyMembersAsync(int companyId)
        {
            IEnumerable<UserDTO> users = await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>("api/companies/members") ?? [];

            return users;
        }

        public async Task<string> GetUserRoleAsync(string userId, int companyId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/companies/members/member/{userId}");
            response.EnsureSuccessStatusCode();

            string userRole = await response.Content.ReadAsStringAsync();
            return userRole;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            IEnumerable<UserDTO> users = await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>($"api/companies/members/{roleName}") ?? [];

            return users;
        }

        public async Task UpdateCompanyAsync(CompanyDTO companyDTO, string adminId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<CompanyDTO>("api/companies", companyDTO);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserRoleAsync(UserDTO user, string adminId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<UserDTO>("api/companies/members/member/role", user);
            response.EnsureSuccessStatusCode();
        }
    }
}
