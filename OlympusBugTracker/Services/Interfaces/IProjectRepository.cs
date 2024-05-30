﻿using OlympusBugTracker.Models;

namespace OlympusBugTracker.Services.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> AddProjectAsync(Project project, int companyId);

        Task<IEnumerable<Project>> GetAllProjectsAsync(int companyId);

        Task<IEnumerable<Project>> GetArchivedProjects(int companyId);

        Task ArchiveProjectAsync(int projectId, int companyId);

        Task RestoreProjectAsync(int projectId, int companyId);

    }
}
