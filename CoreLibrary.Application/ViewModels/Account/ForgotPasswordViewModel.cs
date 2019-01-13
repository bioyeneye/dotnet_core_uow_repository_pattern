using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.Application.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
