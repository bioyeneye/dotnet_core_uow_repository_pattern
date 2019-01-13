using System;
using Microsoft.AspNetCore.Identity;

namespace CoreLibrary.Application.Model.Identities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEnabled { get; set; }
    }
}
