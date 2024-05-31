﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OlympusBugTracker.Data;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;

namespace OlympusBugTracker.Services
{
    public class ProjectRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IProjectRepository
    {
        public async Task<Project> AddProjectAsync(Project project, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            project.Created = DateTimeOffset.Now;
            project.CompanyId = companyId;

            context.Projects.Add(project);
            await context.SaveChangesAsync();

            return project;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync(int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Project> projects = await context.Projects.Where(p => p.CompanyId == companyId && !p.Archived).OrderByDescending(p => p.Created).ToListAsync();

            return projects;
        }

        public async Task<IEnumerable<Project>> GetArchivedProjectsAsync(int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Project> projects = await context.Projects.Where(p => p.CompanyId == companyId && p.Archived).OrderByDescending(p => p.Created).ToListAsync();

            return projects;
        }

        public async Task<Project?> GetProjectByIdAsync(int projectId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Project? project = await context.Projects.Where(p => p.CompanyId == companyId).FirstOrDefaultAsync(p => p.Id == projectId);

            return project;
        }

        public async Task UpdateProjectAsync(Project project, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldEdit = await context.Projects.AnyAsync(p => p.Id == project.Id && p.CompanyId == companyId);

            if (shouldEdit)
            {
                context.Projects.Update(project);
                await context.SaveChangesAsync();
            }
        }

        public async Task ArchiveProjectAsync(int projectId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Project? project = await context.Projects.Include(p => p.Tickets).FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);

            if (project is not null)
            {
                // Archive the tickets using ArchivedByProject

                project.Archived = true;

                context.Projects.Update(project);
                await context.SaveChangesAsync();
            }
        }

        public async Task RestoreProjectAsync(int projectId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Project? project = await context.Projects.Include(p => p.Tickets).FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);

            if (project is not null)
            {
                // Restore the tickets using ArchivedByProject

                project.Archived = false;

                context.Projects.Update(project);
                await context.SaveChangesAsync();
            }
        }

    }
}
