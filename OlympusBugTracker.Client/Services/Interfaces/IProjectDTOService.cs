using OlympusBugTracker.Client.Models;

namespace OlympusBugTracker.Client.Services.Interfaces
{
    public interface IProjectDTOService
    {
        Task<ProjectDTO> AddProjectAsync(ProjectDTO projectDTO, int companyId);

        Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId);

        Task<IEnumerable<ProjectDTO>> GetArchivedProjectsAsync(int companyId);

        Task<ProjectDTO?> GetProjectByIdAsync(int projectId, int companyId);

        Task UpdateProjectAsync(ProjectDTO projectDTO, int companyId);


        Task ArchiveProjectAsync(int projectId, int companyId);

        Task RestoreProjectAsync(int projectId, int companyId);

    }
}
