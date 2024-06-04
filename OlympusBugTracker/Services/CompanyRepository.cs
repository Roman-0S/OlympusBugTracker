﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OlympusBugTracker.Data;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;
using static OlympusBugTracker.Client.Models.Enums;

namespace OlympusBugTracker.Services
{
    public class CompanyRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IServiceProvider serviceProvider) : ICompanyRepository
    {
        public async Task AddUserToRoleAsync(string userId, string roleName, string adminId)
        {
            if (userId == adminId) return;

            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? admin = await userManager.FindByIdAsync(adminId);

            if (admin is not null && await userManager.IsInRoleAsync(admin, nameof(Roles.Admin)))
            {
                ApplicationUser? user = await userManager.FindByIdAsync(userId);

                if (user is not null && user.CompanyId ==  admin.CompanyId)
                {
                    IList<string> currentRoles = await userManager.GetRolesAsync(user);
                    string? currentRole = currentRoles.FirstOrDefault(r => r != nameof(Roles.DemoUser));

                    if (string.Equals(currentRole, roleName, StringComparison.OrdinalIgnoreCase)) return;

                    if (!string.IsNullOrEmpty(currentRole))
                    {
                        await userManager.RemoveFromRoleAsync(user, currentRole);
                    }

                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        public async Task<Company?> GetCompanyByIdAsync(int Id)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Company? company = await context.Companies.Include(c => c.Projects)
                                                      .Include(c => c.Users)
                                                      .Include(c => c.Invites)
                                                      .FirstOrDefaultAsync(c => c.Id == Id);

            return company;
        }

        public async Task<string> GetUserRoleAsync(string userId, int companyId)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? user = await userManager.FindByIdAsync(userId);

            string role = "Unknown";

            if (user?.CompanyId == companyId)
            {
                IList<string> roles = await userManager.GetRolesAsync(user);

                role = roles.FirstOrDefault(r => r != nameof(Roles.DemoUser), role);
            }

            return role;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IList<ApplicationUser> users = await userManager.GetUsersInRoleAsync(roleName);
            IEnumerable<ApplicationUser> companyUsers = users.Where(u => u.CompanyId == companyId);

            return companyUsers;
        }

        public async Task UpdateCompanyAsync(Company company, string adminId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? admin = await userManager.FindByIdAsync(adminId);

            if (admin is not null)
            {
                if (admin.CompanyId == company.Id)
                {
                    context.Companies.Update(company);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
