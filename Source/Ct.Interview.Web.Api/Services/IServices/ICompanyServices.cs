using Ct.Interview.Web.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ct.Interview.Web.Api.Services.IServices
{
    public interface ICompanyServices
    {
        Task<AsxCompany> GetByAsxCode(string asxCode);
    }
}
