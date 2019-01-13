using CoreLibrary.Application.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLibrary.Application
{
    public static class CustomServiceInjector
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                string useSqLite = Startup.Configuration["Data:useSqLite"];
                string useInMemory = Startup.Configuration["Data:useInMemory"];
                if (useInMemory.ToLower() == "true")
                {
                    options.UseInMemoryDatabase("AspNetCoreSpa"); // Takes database name
                }
                else if (useSqLite.ToLower() == "true")
                {
                    /*var connection = Startup.Configuration["Data:SqlLiteConnectionString"];
                    options.UseSqlite(connection);
                    options.UseSqlite(connection, b => b.MigrationsAssembly("AspNetCoreSpa.Web"));*/

                }
                else
                {
                    var connection = Startup.Configuration["Data:SqlServerConnectionString"];
                    options.UseSqlServer(connection);
                    options.UseSqlServer(connection, b => b.MigrationsAssembly("AspNetCoreSpa.Web"));
                }
                options.UseOpenIddict();
            });
            return services;
        }
    }
}
