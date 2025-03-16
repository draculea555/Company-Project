using Company_API.Entities;
using Company_API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Company_API.Repositories
{
    public class CompanyRepository: ICompanyRepository
    {
        private readonly ApiDbContext _context;
        public CompanyRepository(ApiDbContext context) 
        {
            _context = context;
        }
        
        public async Task<int> AddCompanyAsync(Company company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            _context.Companies.Add(company);
            return await _context.SaveChangesAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int companyId)
        {
            return await _context.Companies.FindAsync(companyId);
        }

        public async Task<Company?> GetCompanyByIsinAsync(string companyIsin)
        {
            return await _context.Companies.FirstOrDefaultAsync(x=>x.Isin == companyIsin);
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<bool> UpdateCompanyAsync(Company company)
        {
            var existingCompany = await _context.Companies.FindAsync(company.Id);
            if (existingCompany == null)
                return false;

            _context.Entry(existingCompany).CurrentValues.SetValues(company);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCompanyWebsiteAsync(int companyId, string website)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
                return false;

            company.Website = website;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
