using OlympusBugTracker.Data;
using OlympusBugTracker.Models;

namespace OlympusBugTracker.Services.Interfaces
{
    public interface IProjectRepository
    {

        #region Project

        Task<Project> AddProjectAsync(Project project, int companyId);

        Task<IEnumerable<Project>> GetAllProjectsAsync(int companyId);

        Task<IEnumerable<Project>> GetArchivedProjectsAsync(int companyId);

        Task<Project?> GetProjectByIdAsync(int projectId, int companyId);

        Task UpdateProjectAsync(Project project, int companyId);

        Task ArchiveProjectAsync(int projectId, int companyId);

        Task RestoreProjectAsync(int projectId, int companyId);

        #endregion

        #region Project Members

        Task<IEnumerable<ApplicationUser>> GetProjectMembersAsync(int projectId, int companyId);

        Task<ApplicationUser?> GetProjectManagerAsync(int projectId, int companyId);

        Task AddMemberToProjectAsync(int projectId, string userId, string managerId);

        Task RemoveMemberFromProjectAsync(int projectId, string userId, string managerId);

        Task AssignProjectManagerAsync(int projectId, string userId, string adminId);

        Task RemoveProjectManagerAsync(int projectId, string adminId);

        #endregion

    }
}
