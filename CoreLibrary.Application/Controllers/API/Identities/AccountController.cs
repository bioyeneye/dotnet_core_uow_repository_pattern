using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using CoreLibrary.Application.Model;
using CoreLibrary.Application.Model.Identities;
using CoreLibrary.Application.ViewModels.Account;
using CoreLibrary.BusinessLogic.Utilities;
using CoreLibrary.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreLibrary.Application.Controllers.API.Identities
{
    [Authorize]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IEmailSender _emailSender;
        private static bool _databaseChecked;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext applicationDbContext, IEmailSender emailSender)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
            _emailSender = emailSender;
        }

        //
        // POST: /Account/Register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            EnsureDatabaseCreated(_applicationDbContext);
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var officeClaim = new Claim(OpenIdConnectConstants.Claims.Username, user.Email, ClaimValueTypes.Email);
                    await _userManager.AddClaimAsync(user, officeClaim);

                    var roleAddResult = await _userManager.AddToRoleAsync(user, Constants.UserRoles.User.ToString());
                    if (roleAddResult.Succeeded)
                    {
                         
//                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//                        var host = Request.Scheme + "://" + Request.Host;
//                        var callbackUrl = host + "?userId=" + user.Id + "&emailConfirmCode=" + code;
//                        var confirmationLink = "<a class='btn-primary' href=\"" + callbackUrl + "\">Confirm email address</a>";
//                        await _emailSender.SendEmailAsync(model.Email, "Registration confirmation email", confirmationLink);
                        return Ok();
                    }
                }
                AddErrors(result);
            }

            // If we got this far, something failed.
            return BadRequest(ModelState);
        }

        #region Helpers

        private static void EnsureDatabaseCreated(ApplicationDbContext context)
        {
            if (!_databaseChecked)
            {
                _databaseChecked = true;
                context.Database.EnsureCreated();
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}
