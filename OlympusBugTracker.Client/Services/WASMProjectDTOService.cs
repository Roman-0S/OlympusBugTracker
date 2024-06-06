using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace OlympusBugTracker.Client.Services
{
    public class WASMProjectDTOService : IProjectDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMProjectDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Project

        public async Task<ProjectDTO> AddProjectAsync(ProjectDTO projectDTO, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<ProjectDTO>($"api/projects", projectDTO);
            response.EnsureSuccessStatusCode();

            ProjectDTO? projectCreated = await response.Content.ReadFromJsonAsync<ProjectDTO>();
            return projectCreated!;
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId)
        {
            IEnumerable<ProjectDTO> projects = await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDTO>>("api/projects") ?? [];

            return projects;
        }

        public async Task<IEnumerable<ProjectDTO>> GetArchivedProjectsAsync(int companyId)
        {
            IEnumerable<ProjectDTO> projects = await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDTO>>("api/projects/archived") ?? [];

            return projects;
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(int projectId, int companyId)
        {
            ProjectDTO? project = await _httpClient.GetFromJsonAsync<ProjectDTO>($"api/projects/project/{projectId}");

            return project;
        }

        public async Task UpdateProjectAsync(ProjectDTO projectDTO, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<ProjectDTO>($"api/projects/{projectDTO.Id}", projectDTO);
            response.EnsureSuccessStatusCode();
        }

        public async Task ArchiveProjectAsync(int projectId, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<string>($"api/projects/archive/{projectId}", "");

            response.EnsureSuccessStatusCode();
        }

        public async Task RestoreProjectAsync(int projectId, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<string>($"api/projects/restore/{projectId}", "");

            response.EnsureSuccessStatusCode();
        }


        #endregion

        #region Project Managers

        public Task<IEnumerable<UserDTO>> GetProjectMembersAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO?> GetProjectManagerAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task AddMemberToProjectAsync(int projectId, string memberId, string managerId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveMemberFromProjectAsync(int projectId, string memberId, string managerId)
        {
            throw new NotImplementedException();
        }

        public Task AssignProjectManagerAsync(int projectId, string memberId, string adminId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveProjectManagerAsync(int projectId, string adminId)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
