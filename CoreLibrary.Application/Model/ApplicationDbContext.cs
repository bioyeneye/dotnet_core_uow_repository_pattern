using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLibrary.Application.Model.Identities;
using CoreLibrary.Utilities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreLibrary.Application.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // get the configuration from the app settings
            //IConfigurationRoot config = ApplicationUtilities.GetApplicationConfiguration("");

            // define the database to use
            //optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
