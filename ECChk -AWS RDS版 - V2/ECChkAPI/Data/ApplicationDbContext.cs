using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECChkAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECChkAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<IFECCUTF> IFECCUTFs { get; set; }
    }
}
