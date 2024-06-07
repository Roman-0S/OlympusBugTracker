using OlympusBugTracker.Client.Models;

namespace OlympusBugTracker.Client.Services.Interfaces
{
    public interface IProjectDTOService
    {

        #region Project

        Task<ProjectDTO> AddProjectAsync(ProjectDTO projectDTO, int companyId);

        Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId);

        Task<IEnumerable<ProjectDTO>> GetArchivedProjectsAsync(int companyId);

        Task<ProjectDTO?> GetProjectByIdAsync(int projectId, int companyId);

        Task UpdateProjectAsync(ProjectDTO projectDTO, int companyId);

        Task ArchiveProjectAsync(int projectId, int companyId);

        Task RestoreProjectAsync(int projectId, int companyId);

        #endregion

        #region Project Manager

        Task<IEnumerable<ProjectDTO>> GetMemberProjectsAsync(string userId, int companyId);

        Task<IEnumerable<UserDTO>> GetProjectMembersAsync(int projectId, int companyId);

        Task<UserDTO?> GetProjectManagerAsync(int projectId, int companyId);

        Task AddMemberToProjectAsync(int projectId, string memberId, string managerId);

        Task RemoveMemberFromProjectAsync(int projectId, string memberId, string managerId);

        Task AssignProjectManagerAsync(int projectId, string memberId, string adminId);

        Task RemoveProjectManagerAsync(int projectId, string adminId);

        #endregion

    }
}
