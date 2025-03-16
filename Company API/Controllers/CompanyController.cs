using Company_API.Entities;
using Company_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Company_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService companyService;
        private readonly ILogger<CompanyController> logger;

        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            this.companyService = companyService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("AllCompanies")]
        public ActionResult<List<Company>> GetAllCompanies()
        {
            try
            { 
                return new ActionResult<List<Company>>(companyService.RetrieveAll().GetAwaiter().GetResult());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve the company list");
            }
            return StatusCode(500, new { message = "Failed to retrieve the company list" });
        }

        [HttpGet]
        [Route("Id/{CompanyId}")]
        public ActionResult<Company> Get(int CompanyId)
        {
            try
            {
                Company result = companyService.Retrieve(CompanyId).GetAwaiter().GetResult();
                if (result != null)
                {
                    return new ActionResult<Company>(result);
                }
                else
                    return NotFound(new { message = $"Company with ID {CompanyId} not found." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve the company");
            }
            return StatusCode(500, new { message = "Failed to retrieve the company" });
        }

        [HttpGet]
        [Route("Isin/{CompanyIsin}")]
        public ActionResult<Company> Get(string CompanyIsin)
        {
            try
            {
                Company result = companyService.Retrieve(CompanyIsin).GetAwaiter().GetResult();
                if (result != null)
                {
                    return new ActionResult<Company>(result);
                }
                else
                    return StatusCode(404, new { message = $"Company with Isin {CompanyIsin} not found." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve the company");
            }
            return StatusCode(500, new { message = "Failed to retrieve the company" });
        }

        [HttpPost]
        [Route("Create")]
        public ActionResult<int> Create([FromBody] Company company)
        {
            if (company == null)
            {
                return StatusCode(400, new { message = "Invalid company data." });
            }
            try
            {
                int result = companyService.Create(company).GetAwaiter().GetResult();
                if (result == 1)
                {
                    return new ActionResult<int>(company.Id);
                }
            }
            catch (ArgumentException a)
            {
                return StatusCode(500, a.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create company");
            }
            return StatusCode(500, new { message = "Failed to create company" });
        }

        [HttpPut]
        [Route("Update/{CompanyId}")]
        public ActionResult<bool> Update(int CompanyId, [FromBody] Company company)
        {
            if (company == null)
            {
                return StatusCode(400, new { message = "Invalid company data." });
            }
            try
            {
                bool result = companyService.Update(company).GetAwaiter().GetResult();
                if (result)
                {
                    return new ActionResult<bool>(result);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update the company");
            }
            return StatusCode(500, new { message = "Failed to update the company" });
        }

        [HttpPatch]
        [Route("UpdateWebsite/{CompanyId}")]
        public ActionResult<bool> UpdateWebsite(int CompanyId, [FromBody] string Website)
        {
            if (Website == null)
            {
                return StatusCode(400, new { message = "Missing website url." });
            }
            try
            {
                bool result = companyService.UpdateWebsite(CompanyId, Website).GetAwaiter().GetResult();
                if (result)
                {
                    return new ActionResult<bool>(result);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update the company website");
            }
            return StatusCode(500, new { message = "Failed to update the company website" });
        }
    }
}
