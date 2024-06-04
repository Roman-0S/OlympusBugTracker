using OlympusBugTracker.Client.Models;

namespace OlympusBugTracker.Client.Services.Interfaces
{
    public interface ICompanyDTOService
    {
        Task<IEnumerable<UserDTO>> GetCompanyMembersAsync(int companyId);

        Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName, int companyId);

        Task<CompanyDTO?> GetCompanyByIdAsync(int companyId);

        Task UpdateUserRoleAsync(UserDTO user, string adminId);

        Task UpdateCompanyAsync(CompanyDTO companyDTO, string adminId);

    }
}
