using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using OlympusBugTracker.Data;
using OlympusBugTracker.Helpers;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;

namespace OlympusBugTracker.Services
{
    public class CompanyDTOService(ICompanyRepository repository) : ICompanyDTOService
    {
        public async Task<CompanyDTO?> GetCompanyByIdAsync(int companyId)
        {
            Company? company = await repository.GetCompanyByIdAsync(companyId);

            return company?.ToDTO();
        }

        public async Task<IEnumerable<UserDTO>> GetCompanyMembersAsync(int companyId)
        {
            Company? company = await repository.GetCompanyByIdAsync(companyId);

            if (company is null) return [];

            List<UserDTO> members = [];

            foreach(ApplicationUser user in company.Users)
            {
                UserDTO member = user.ToDTO();
                member.Role = await repository.GetUserRoleAsync(user.Id, companyId);
                members.Add(member);
            }

            return members;
        }

        public async Task<string> GetUserRoleAsync(string userId, int companyId)
        {
            string? role = await repository.GetUserRoleAsync(userId, companyId);

            return role;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            IEnumerable<ApplicationUser> users = await repository.GetUsersInRoleAsync(roleName, companyId);

            IEnumerable<UserDTO> userDTOs = users.Select(u => u.ToDTO());

            foreach (UserDTO user in userDTOs)
            {
                user.Role = roleName;
            }

            return userDTOs;
        }

        public async Task UpdateCompanyAsync(CompanyDTO companyDTO, string adminId)
        {
            Company? company = await repository.GetCompanyByIdAsync(companyDTO.Id);

            if (company is not null)
            {
                company.Name = companyDTO.Name;
                company.Description = companyDTO.Description;


                if (companyDTO.ImageURL.StartsWith("data:"))
                {
                    company.Image = UploadHelper.GetImageUpload(companyDTO.ImageURL);
                }

                await repository.UpdateCompanyAsync(company, adminId);
            }

        }

        public async Task UpdateUserRoleAsync(UserDTO user, string adminId)
        {
            if (string.IsNullOrEmpty(user.Role)) return;

            await repository.AddUserToRoleAsync(user.Id!, user.Role, adminId);
        }
    }
}
