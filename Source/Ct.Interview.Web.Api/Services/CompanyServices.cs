using CsvHelper;
using Ct.Interview.Web.Api.Models;
using Ct.Interview.Web.Api.Services.IServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ct.Interview.Web.Api.Services
{
    public class CompanyServices: ICompanyServices
    {
        private const string companyListCacheKey = "companyList";
        private readonly ILogger<CompanyServices> _logger;
        private readonly IConfiguration _configuration;
        private IMemoryCache _cache;

        public CompanyServices(ILogger<CompanyServices> logger, IConfiguration config, IMemoryCache cache)
        {
            _logger = logger;
            _configuration = config;
            _cache = cache;
        }

        public async Task<AsxCompany> GetByAsxCode(string asxCode)
        {
            AsxCompany result = null;
            IEnumerable<AsxCompany> records = null;
            try
            {
                if (_cache.TryGetValue(companyListCacheKey, out List<AsxCompany> companies))
                {
                    result = companies.FirstOrDefault(a => string.Compare(a.AsxCode, asxCode, true) == 0);

                } else
                {
                    var csvEndpoint = _configuration.GetValue<string>("AsxSettings:ListedSecuritiesCsvUrl");

                    if (csvEndpoint != null)
                    {
                        using (var httpClient = new HttpClient())
                        {
                            using (var stream = await httpClient.GetStreamAsync(csvEndpoint))
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                                    {
                                        records = csv.GetRecords<AsxCompany>();

                                        if (records != null)
                                        {
                                            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                                                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                                                .SetPriority(CacheItemPriority.Normal)
                                                .SetSize(1);

                                            var listRecords = records.ToList();

                                            result = listRecords.FirstOrDefault(a => string.Compare(a.AsxCode, asxCode, true) == 0);

                                            _cache.Set(companyListCacheKey, listRecords, cacheEntryOptions);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }      
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception at CompanyServices:GetByAsxCode.");
            }

            return await Task.FromResult(result);
        }
    }
}
