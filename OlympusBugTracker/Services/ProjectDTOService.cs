using Microsoft.AspNetCore.Authorization.Infrastructure;
using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using OlympusBugTracker.Data;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;
using static OlympusBugTracker.Client.Models.Enums;

namespace OlympusBugTracker.Services
{
    public class ProjectDTOService(IProjectRepository repository, ICompanyRepository companyRepository) : IProjectDTOService
    {

        #region Project

        public async Task<ProjectDTO> AddProjectAsync(ProjectDTO projectDTO, int companyId)
        {
            Project project = new()
            {
                Name = projectDTO.Name,
                Description = projectDTO.Description,
                StartDate = projectDTO.StartDate,
                EndDate = projectDTO.EndDate,
                Priority = projectDTO.Priority,
            };

            project = await repository.AddProjectAsync(project, companyId);

            return project.ToDTO();
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId)
        {
            IEnumerable<Project> projects = await repository.GetAllProjectsAsync(companyId);

            return projects.Select(p => p.ToDTO());
        }

        public async Task<IEnumerable<ProjectDTO>> GetArchivedProjectsAsync(int companyId)
        {
            IEnumerable<Project> projects = await repository.GetArchivedProjectsAsync(companyId);

            return projects.Select(p => p.ToDTO());
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(int projectId, int companyId)
        {
            Project? project = await repository.GetProjectByIdAsync(projectId, companyId);

            return project?.ToDTO();
        }

        public async Task UpdateProjectAsync(ProjectDTO projectDTO, int companyId)
        {
            Project? project = await repository.GetProjectByIdAsync(projectDTO.Id, companyId);

            if (project is not null)
            {
                project.Name = projectDTO.Name;
                project.Description = projectDTO.Description;
                project.StartDate = projectDTO.StartDate;
                project.EndDate = projectDTO.EndDate;
                project.Priority = projectDTO.Priority;

                await repository.UpdateProjectAsync(project, companyId);
            }
        }

        public async Task ArchiveProjectAsync(int projectId, int companyId)
        {
            await repository.ArchiveProjectAsync(projectId, companyId);
        }

        public async Task RestoreProjectAsync(int projectId, int companyId)
        {
            await repository.RestoreProjectAsync(projectId, companyId);
        }


        #endregion

        #region Project Managers

        public async Task<IEnumerable<ProjectDTO>> GetMemberProjectsAsync(string userId, int companyId)
        {
            IEnumerable<Project> projects = await repository.GetMemberProjectsAsync(userId, companyId);

            return projects.Select(p => p.ToDTO());
        }

        public async Task<IEnumerable<UserDTO>> GetProjectMembersAsync(int projectId, int companyId)
        {
            IEnumerable<ApplicationUser> members = await repository.GetProjectMembersAsync(projectId, companyId);

            List<UserDTO> membersDTO = [];

            foreach(ApplicationUser member in members)
            {
                UserDTO userDTO = member.ToDTO();
                userDTO.Role = await companyRepository.GetUserRoleAsync(member.Id, companyId);
                membersDTO.Add(userDTO);
            }

            return membersDTO;
        }

        public async Task<UserDTO?> GetProjectManagerAsync(int projectId, int companyId)
        {
            ApplicationUser? projectManager = await repository.GetProjectManagerAsync(projectId, companyId);

            if (projectManager is null) return null;

            UserDTO userDTO = projectManager.ToDTO();
            userDTO.Role = nameof(Roles.ProjectManager);

            return userDTO;
        }

        public async Task AddMemberToProjectAsync(int projectId, string memberId, string managerId)
        {
            await repository.AddMemberToProjectAsync(projectId, memberId, managerId);
        }

        public async Task RemoveMemberFromProjectAsync(int projectId, string memberId, string managerId)
        {
            await repository.RemoveMemberFromProjectAsync(projectId, memberId, managerId);
        }

        public async Task AssignProjectManagerAsync(int projectId, string memberId, string adminId)
        {
            await repository.AssignProjectManagerAsync(projectId, memberId, adminId);
        }

        public async Task RemoveProjectManagerAsync(int projectId, string adminId)
        {
            await repository.RemoveProjectManagerAsync(projectId, adminId);
        }

        #endregion

    }
}
