using OlympusBugTracker.Client.Models;

namespace OlympusBugTracker.Client.Services.Interfaces
{
    public interface ICompanyDTOService
    {
        Task<IEnumerable<UserDTO>> GetCompanyMembersAsync(int companyId);
    }
}
