using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using OlympusBugTracker.Data;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;

namespace OlympusBugTracker.Services
{
    public class CompanyDTOService(ICompanyRepository repository) : ICompanyDTOService
    {
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
    }
}
