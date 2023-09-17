using Ct.Interview.Web.Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ct.Interview.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : Controller
    {
        private readonly ICompanyServices _companyServices;
        public CompaniesController(ICompanyServices companyServices) {
            this._companyServices = companyServices;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByCode(string id)
        {
            var company = await _companyServices.GetByAsxCode(id);

            if (company == null)
                return NotFound();
            else
                return Ok(company);
        }
    }
}
