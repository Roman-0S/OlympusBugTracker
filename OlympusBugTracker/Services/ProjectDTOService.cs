using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;

namespace OlympusBugTracker.Services
{
    public class ProjectDTOService(IProjectRepository repository) : IProjectDTOService
    {

        public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId)
        {
            IEnumerable<Project> projects = await repository.GetAllProjectsAsync(companyId);

            return projects.Select(p => p.ToDTO());
        }

        public async Task<IEnumerable<ProjectDTO>> GetArchivedProjects(int companyId)
        {
            IEnumerable<Project> projects = await repository.GetArchivedProjects(companyId);

            return projects.Select(p => p.ToDTO());
        }

        public async Task ArchiveProjectAsync(int projectId, int companyId)
        {
            await repository.ArchiveProjectAsync(projectId, companyId);
        }

        public async Task RestoreProjectAsync(int projectId, int companyId)
        {
            await repository.RestoreProjectAsync(projectId, companyId);
        }
    }
}
