using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using CoreLibrary.Application.Model;
using CoreLibrary.Application.Model.Identities;
using CoreLibrary.BusinessLogic.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;

namespace CoreLibrary.Application.App_Start
{
    public class DefaultUserModel
    {
        public string RoleName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }

        public int RoleId { get; set; }
    }
    public interface IDatabaseInitializer
    {
        Task SeedAsync(IConfiguration configuration);
    }

    public class DatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly OpenIddictApplicationManager<OpenIddictApplication> _openIddictApplicationManager;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DatabaseInitializer(
            ApplicationDbContext context,
            ILogger<DatabaseInitializer> logger,
            OpenIddictApplicationManager<OpenIddictApplication> openIddictApplicationManager,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment hostingEnvironment
        )
        {
            _context = context;
            _logger = logger;
            _openIddictApplicationManager = openIddictApplicationManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task SeedAsync(IConfiguration configuration)
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            CreateRoles();
            CreateUsers();
            await AddOpenIdConnectOptions(configuration);
        }

        private void CreateRoles()
        {
            var roles = Enum.GetNames(typeof(Constants.UserRoles));
            foreach (var role in roles)
            {
                if (!_roleManager.RoleExistsAsync(role).Result)
                {
                    _roleManager.CreateAsync(new ApplicationRole() {Name = role}).Result.ToString();
                }
            }
        }

        private void CreateUsers()
        {
            if (!_context.ApplicationUsers.Any())
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    FirstName = "Admin first",
                    LastName = "Admin last",
                    Email = "admin@admin.com",
                    PhoneNumber = "0123456789",
                    EmailConfirmed = true,
                    CreatedDate = DateTime.Now,
                    IsEnabled = true
                };
                _userManager.CreateAsync(adminUser, "P@ssw0rd!").Result.ToString();
                _userManager.AddClaimAsync(adminUser,
                        new Claim(OpenIdConnectConstants.Claims.PhoneNumber, adminUser.PhoneNumber,
                            ClaimValueTypes.String))
                    .Result.ToString();
                _userManager
                    .AddToRoleAsync(_userManager.FindByNameAsync("admin@admin.com").GetAwaiter().GetResult(), "Admin")
                    .Result.ToString();

                var normalUser = new ApplicationUser
                {
                    UserName = "user@user.com",
                    FirstName = "First",
                    LastName = "Last",
                    Email = "user@user.com",
                    PhoneNumber = "0123456789",
                    EmailConfirmed = true,
                    CreatedDate = DateTime.Now,
                    IsEnabled = true
                };
                _userManager.CreateAsync(normalUser, "P@ssw0rd!").Result.ToString();
                _userManager.AddClaimAsync(adminUser,
                    new Claim(OpenIdConnectConstants.Claims.PhoneNumber, normalUser.PhoneNumber,
                        ClaimValueTypes.String)).Result.ToString();
                _userManager
                    .AddToRoleAsync(_userManager.FindByNameAsync("user@user.com").GetAwaiter().GetResult(), "User")
                    .Result.ToString();
            }
        }


        private async Task AddOpenIdConnectOptions(IConfiguration configuration)
        {
            if (await _openIddictApplicationManager.FindByClientIdAsync("aspnetcorespa") == null)
            {
                var host = configuration["HostUrl"].ToString();

                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = "aspnetcorespa",
                    DisplayName = "AspnetCoreSpa",
                    PostLogoutRedirectUris = {new Uri($"{host}signout-oidc")},
                    RedirectUris = {new Uri(host)},
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Implicit,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken
                    }
                };

                await _openIddictApplicationManager.CreateAsync(descriptor);
            }
        }
    }
}