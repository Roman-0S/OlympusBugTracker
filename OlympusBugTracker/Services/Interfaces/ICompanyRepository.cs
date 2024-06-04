using OlympusBugTracker.Data;
using OlympusBugTracker.Models;

namespace OlympusBugTracker.Services.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company?> GetCompanyByIdAsync(int Id);

        Task<string> GetUserRoleAsync(string userId, int companyId);

        Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName, int companyId);

        Task AddUserToRoleAsync(string userId, string roleName, string adminId);

        Task UpdateCompanyAsync(Company company, string adminId);

    }
}
