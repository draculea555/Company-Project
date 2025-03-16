using Company_API.Entities;
using Company_API.Interfaces;
using System.ComponentModel.Design;

namespace Company_API.Services
{
    public class CompanyService(ICompanyRepository companyRepository) : ICompanyService
    {
        public async Task<int> Create(Company company)
        {
            // validate ISIN
            if (company.Isin.Length>1 && Char.IsLetter(company.Isin[0]) && Char.IsLetter(company.Isin[1]))
            {
                return await companyRepository.AddCompanyAsync(company);
            }
            else
            {
                throw new ArgumentException("ISIN must start with two letters");
            }
        }

        public async Task<Company?> Retrieve(int companyId)
        {
            return await companyRepository.GetCompanyByIdAsync(companyId);
        }

        public async Task<Company?> Retrieve(string companyIsin)
        {
            return await companyRepository.GetCompanyByIsinAsync(companyIsin);
        }

        public async Task<List<Company>> RetrieveAll()
        {
            return await companyRepository.GetAllCompaniesAsync();
        }

        public async Task<bool> Update(Company company)
        {
            
            if (company.Isin.Length > 1 && Char.IsLetter(company.Isin[0]) && Char.IsLetter(company.Isin[1]))
            {
                return await companyRepository.UpdateCompanyAsync(company);
            }
            else
            {
                throw new ArgumentException("ISIN must start with two letters");
            }
        }

        public async Task<bool> UpdateWebsite(int companyId, string website)
        {
            return await companyRepository.UpdateCompanyWebsiteAsync(companyId, website);
        }
    }
}
