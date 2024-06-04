using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OlympusBugTracker.Data;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;
using static OlympusBugTracker.Client.Models.Enums;

namespace OlympusBugTracker.Services
{
    public class CompanyRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IServiceProvider serviceProvider) : ICompanyRepository
    {
        public Task AddUserToRoleAsync(string userId, string roleName, string adminId)
        {
            throw new NotImplementedException();
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

        public Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCompanyAsync(Company company, string adminId)
        {
            throw new NotImplementedException();
        }
    }
}
