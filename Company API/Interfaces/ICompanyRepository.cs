using Company_API.Entities;

namespace Company_API.Interfaces
{
    public interface ICompanyRepository
    {
        Task<int> AddCompanyAsync(Company company);
        Task<Company?> GetCompanyByIdAsync(int companyId);
        Task<Company?> GetCompanyByIsinAsync(string companyIsin);
        Task<List<Company>> GetAllCompaniesAsync();
        Task<bool> UpdateCompanyAsync(Company company);
        Task<bool> UpdateCompanyWebsiteAsync(int companyId, string website);
    }
}
