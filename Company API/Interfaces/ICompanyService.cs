using Company_API.Entities;

namespace Company_API.Interfaces
{
    public interface ICompanyService
    {
        Task<int> Create(Company company);
        Task<Company?> Retrieve(int companyId);
        Task<Company?> Retrieve(string companyIsin);
        Task<List<Company>> RetrieveAll();
        Task<bool> Update(Company company);
        Task<bool> UpdateWebsite(int companyId, string website);
    }
}
