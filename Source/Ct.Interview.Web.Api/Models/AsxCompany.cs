using CsvHelper.Configuration.Attributes;

namespace Ct.Interview.Web.Api.Models
{
    public class AsxCompany
    {
        public AsxCompany() { }

        public AsxCompany(string companyName, string asxCode, string gicsIndustryGroup)
        {
            AsxCode = asxCode;
            CompanyName = companyName;
            GicsIndustryGroup = gicsIndustryGroup;
        }

        [Index(0)]
        public string CompanyName { get; set; }
        [Index(1)]
        public string AsxCode { get; set; }
        [Index(2)]
        public string GicsIndustryGroup { get; set; }
    }
}
