using Ct.Interview.Web.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ct.Interview.Web.Api.Data
{
    public class DataContext : DbContext
    {
        public virtual DbSet<AsxCompany> AsxCompany { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AsxCompany>().HasKey(x => x.AsxCode);
        }
    }
}
