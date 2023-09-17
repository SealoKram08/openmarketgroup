using Ct.Interview.Web.Api.Controllers;
using Ct.Interview.Web.Api.Models;
using Ct.Interview.Web.Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Ct.Interview.Web.Api.UnitTest
{
    public class CompaniesControllerUnitTest
    {
        private readonly Mock<ICompanyServices> _companyServicesMock;

        public CompaniesControllerUnitTest()
        {
            _companyServicesMock = new Mock<ICompanyServices>();
        }

        [Fact]
        public async void GetByCode_HasValidCode_ReturnsCompanyDetails()
        {
            //Arrange
            var companies = GetAsxCompanies();
            _companyServicesMock.Setup(x => x.GetByAsxCode("TDO")).ReturnsAsync(companies[0]).Verifiable();

            var companyController = new CompaniesController(_companyServicesMock.Object);

            //Act
            var response = await companyController.GetByCode("TDO") as OkObjectResult;

            //Asset
            var company = response?.Value as AsxCompany;

            Assert.NotNull(company);
            Assert.Equal(companies[0].AsxCode, company.AsxCode);
            Assert.True(companies[0].AsxCode == company.AsxCode);
        }

        [Fact]
        public async void GetByCode_HasInValidCode_Returns404NotFound()
        {
            //Arrange
            var companyController = new CompaniesController(_companyServicesMock.Object);

            //Act
            var response = await companyController.GetByCode("test1");

            //Asset
            Assert.IsType<NotFoundResult>(response);
        }

        private List<AsxCompany> GetAsxCompanies()
        {
            return new List<AsxCompany>() {
                new AsxCompany
                {
                    CompanyName = "3D OIL LIMITED",
                    AsxCode = "TDO",
                    GicsIndustryGroup = "Energy"
                },
                new AsxCompany
                {
                    CompanyName = "1414 DEGREES LIMITED",
                    AsxCode = "14D",
                    GicsIndustryGroup = "Capital Goods"
                },
                new AsxCompany {
                    CompanyName = "29METALS LIMITED",
                    AsxCode = "29M",
                    GicsIndustryGroup = "Materials"
                }
            };
        }
    }
}