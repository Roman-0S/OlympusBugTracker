using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OlympusBugTracker.Data;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;
using static OlympusBugTracker.Client.Models.Enums;

namespace OlympusBugTracker.Services
{
    public class ProjectRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IServiceProvider serviceProvider) : IProjectRepository
    {

        #region Project

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

            Project? project = await context.Projects.Include(p => p.Tickets).Where(p => p.CompanyId == companyId).FirstOrDefaultAsync(p => p.Id == projectId);

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

        #endregion

        #region Project Managers

        public async Task<IEnumerable<Project>> GetMemberProjectsAsync(string userId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Project> projects = await context.Projects.Where(p => p.CompanyId == companyId && p.Users.Any(u => u.Id == userId)).OrderByDescending(p => p.Created).ToListAsync();

            return projects;
        }

        public async Task<IEnumerable<ApplicationUser>> GetProjectMembersAsync(int projectId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Project? project = await context.Projects.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);

            return project?.Users ?? [];
        }

        public async Task<ApplicationUser?> GetProjectManagerAsync(int projectId, int companyId)
        {
            IEnumerable<ApplicationUser> projectMembers = await GetProjectMembersAsync(projectId, companyId);

            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (ApplicationUser user in projectMembers)
            {
                bool isProjectManager = await userManager.IsInRoleAsync(user, nameof(Roles.ProjectManager));

                if (isProjectManager)
                {
                    return user;
                }
            }

            return null;
        }

        public async Task AddMemberToProjectAsync(int projectId, string userId, string managerId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? manager = await userManager.FindByIdAsync(managerId);
            if (manager is null) return;

            bool isAdmin = await userManager.IsInRoleAsync(manager, nameof(Roles.Admin));

            if (!isAdmin)
            {
                ApplicationUser? projectManager = await GetProjectManagerAsync(projectId, manager.CompanyId);

                if (projectManager?.Id != managerId) return;
            }

            ApplicationUser? userToAdd = await context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.CompanyId == manager.CompanyId);
            if (userToAdd is null) return;

            bool userIsProjectManager = await userManager.IsInRoleAsync(userToAdd, nameof(Roles.ProjectManager));
            if (userIsProjectManager) return;

            bool userIsAdmin = await userManager.IsInRoleAsync(userToAdd, nameof(Roles.Admin));
            if (userIsAdmin) return;

            Project? project = await context.Projects.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == manager.CompanyId);
            if (project is null) return;

            if (!project.Users.Any(u => u.Id == userToAdd.Id))
            {
                project.Users.Add(userToAdd);
                await context.SaveChangesAsync();
            }

        }

        public async Task RemoveMemberFromProjectAsync(int projectId, string userId, string managerId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? manager = await userManager.FindByIdAsync(managerId);
            if (manager is null) return;

            Project? project = await context.Projects.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == manager.CompanyId);
            if (project is null) return;

            ApplicationUser? memberToRemove = project.Users.FirstOrDefault(u => u.Id == userId);
            if (memberToRemove is null) return;

            project.Users.Remove(memberToRemove);
            await context.SaveChangesAsync();
        }

        public async Task AssignProjectManagerAsync(int projectId, string userId, string adminId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? admin = await context.Users.FindAsync(adminId);
            if (admin is null) return;

            bool isAdmin = admin is not null && await userManager.IsInRoleAsync(admin, nameof(Roles.Admin));
            bool isPM = admin is not null && await userManager.IsInRoleAsync(admin, nameof(Roles.ProjectManager));

            if (isAdmin || (isPM && userId == adminId))
            {
                ApplicationUser? projectManager = await context.Users.FindAsync(userId);

                if (projectManager is not null && projectManager.CompanyId == admin!.CompanyId && await userManager.IsInRoleAsync(projectManager, nameof(Roles.ProjectManager)))
                {
                    await RemoveProjectManagerAsync(projectId, adminId);

                    Project? project = await context.Projects.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == admin.CompanyId);

                    if (project is not null)
                    {
                        project.Users.Add(projectManager);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task RemoveProjectManagerAsync(int projectId, string adminId)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? admin = await userManager.FindByIdAsync(adminId);
            if (admin is null) return;

            ApplicationUser? projectManager = await GetProjectManagerAsync(projectId, admin.CompanyId);

            if (projectManager is null) return;

            if (await userManager.IsInRoleAsync(admin, nameof(Roles.Admin)))
            {
                await RemoveMemberFromProjectAsync(projectId, projectManager.Id, adminId);
            }
        }

        #endregion

    }
}
