using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Company_API.Entities;
using Company_API.Interfaces;
using Company_API.Services;

namespace Company_API.Tests
{
    public class CompanyServiceTests
    {
        private readonly Mock<ICompanyRepository> _mockCompanyRepository;
        private readonly CompanyService _companyService;

        public CompanyServiceTests()
        {
            // Arrange - Create mock of ICompanyRepository and CompanyService
            _mockCompanyRepository = new Mock<ICompanyRepository>();
            _companyService = new CompanyService(_mockCompanyRepository.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnCompanyId_WhenValidISIN()
        {
            // Arrange
            var company = new Company { Isin = "US1234567890" };
            _mockCompanyRepository.Setup(repo => repo.AddCompanyAsync(company)).ReturnsAsync(1);

            // Act
            var result = await _companyService.Create(company);

            // Assert
            result.Should().Be(1);
            _mockCompanyRepository.Verify(repo => repo.AddCompanyAsync(company), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldThrowArgumentException_WhenInvalidISIN()
        {
            // Arrange
            var company = new Company { Isin = "1234567890" }; // Invalid ISIN (does not start with two letters)

            // Act
            Func<Task> act = async () => await _companyService.Create(company);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("ISIN must start with two letters");
            _mockCompanyRepository.Verify(repo => repo.AddCompanyAsync(It.IsAny<Company>()), Times.Never);
        }

        [Fact]
        public async Task Retrieve_ShouldReturnCompany_WhenCompanyExists()
        {
            // Arrange
            var companyId = 1;
            var company = new Company { Id = companyId, Isin = "US1234567890" };
            _mockCompanyRepository.Setup(repo => repo.GetCompanyByIdAsync(companyId)).ReturnsAsync(company);

            // Act
            var result = await _companyService.Retrieve(companyId);

            // Assert
            result.Should().BeEquivalentTo(company);
            _mockCompanyRepository.Verify(repo => repo.GetCompanyByIdAsync(companyId), Times.Once);
        }

        [Fact]
        public async Task Retrieve_ShouldReturnNull_WhenCompanyDoesNotExist()
        {
            // Arrange
            var companyId = 1;
            _mockCompanyRepository.Setup(repo => repo.GetCompanyByIdAsync(companyId)).ReturnsAsync((Company?)null);

            // Act
            var result = await _companyService.Retrieve(companyId);

            // Assert
            result.Should().BeNull();
            _mockCompanyRepository.Verify(repo => repo.GetCompanyByIdAsync(companyId), Times.Once);
        }

        [Fact]
        public async Task RetrieveByIsin_ShouldReturnCompany_WhenCompanyExists()
        {
            // Arrange
            var companyIsin = "US1234567890";
            var company = new Company { Isin = companyIsin };
            _mockCompanyRepository.Setup(repo => repo.GetCompanyByIsinAsync(companyIsin)).ReturnsAsync(company);

            // Act
            var result = await _companyService.Retrieve(companyIsin);

            // Assert
            result.Should().BeEquivalentTo(company);
            _mockCompanyRepository.Verify(repo => repo.GetCompanyByIsinAsync(companyIsin), Times.Once);
        }

        [Fact]
        public async Task RetrieveAll_ShouldReturnAllCompanies()
        {
            // Arrange
            var companies = new List<Company>
        {
            new Company { Id = 1, Isin = "US1234567890" },
            new Company { Id = 2, Isin = "US9876543210" }
        };
            _mockCompanyRepository.Setup(repo => repo.GetAllCompaniesAsync()).ReturnsAsync(companies);

            // Act
            var result = await _companyService.RetrieveAll();

            // Assert
            result.Should().BeEquivalentTo(companies);
            _mockCompanyRepository.Verify(repo => repo.GetAllCompaniesAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnTrue_WhenValidISIN()
        {
            // Arrange
            var company = new Company { Isin = "US1234567890" };
            _mockCompanyRepository.Setup(repo => repo.UpdateCompanyAsync(company)).ReturnsAsync(true);

            // Act
            var result = await _companyService.Update(company);

            // Assert
            result.Should().BeTrue();
            _mockCompanyRepository.Verify(repo => repo.UpdateCompanyAsync(company), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldThrowArgumentException_WhenInvalidISIN()
        {
            // Arrange
            var company = new Company { Isin = "1234567890" }; // Invalid ISIN

            // Act
            Func<Task> act = async () => await _companyService.Update(company);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("ISIN must start with two letters");
            _mockCompanyRepository.Verify(repo => repo.UpdateCompanyAsync(It.IsAny<Company>()), Times.Never);
        }

        [Fact]
        public async Task UpdateWebsite_ShouldReturnTrue_WhenWebsiteUpdated()
        {
            // Arrange
            var companyId = 1;
            var website = "http://example.com";
            _mockCompanyRepository.Setup(repo => repo.UpdateCompanyWebsiteAsync(companyId, website)).ReturnsAsync(true);

            // Act
            var result = await _companyService.UpdateWebsite(companyId, website);

            // Assert
            result.Should().BeTrue();
            _mockCompanyRepository.Verify(repo => repo.UpdateCompanyWebsiteAsync(companyId, website), Times.Once);
        }
    }
}
