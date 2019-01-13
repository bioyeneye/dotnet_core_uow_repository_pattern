using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.Core.ViewModel
{
    public class UserProfileModel
    {

        public long Id { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }
        [Required]
        public int RoleId { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }
        public int? StateId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TempPassword { get; set; }
        public string ConfirmationCode { get; set; }
        public bool PasswordChanged { get; set; } 
        public string ProfileImageUrl { get; set; }
        public bool IsActive { get; set; }
        public List<int> RoleIds { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string MaritalStatus { get; set; }
        public bool IsSuperAdmin { get; set; }

        public string GetAuditDetails()
        {
            return $"User - {FirstName} {LastName} - {Id}";
        }
    }

    public class UserProfileItem : UserProfileModel
    {
        public DateTime DateCreated { get; set; }
        public string RoleName { get; set; }
        public DateTime? LastActiveTime { get; set; }
    }

    public class UserProfileDetail : UserProfileItem
    {

    }
    public class UserProfileFilter : UserProfileItem
    {
        public static UserProfileFilter Deserilize(string whereCondition)
        {
            UserProfileFilter filter = null;
            if (whereCondition != null)
            {
                //filter = JsonConvert.DeserializeObject<UserProfileFilter>(whereCondition);
            }
            return filter;
        }

        public IEnumerable<int> InRoles { get; set; }
        public string Name { get; set; }
        public DateTime? DateCreatedFrom { get; set; }
        public DateTime? DateCreatedTo { get; set; }
        public IEnumerable<long> UserProfileIds { get; set; }
    }
    public class UserProfileInfo
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public long Id { get; set; }

        public string Role { get; set; }

        public int? RoleId { get; set; }
    }

    
    public class UserProfileCookieInfo : UserProfileInfo
    {
        public IEnumerable<object> UserRoles { get; set; }
    }

    public class UserCreationMessageModel : UserProfileModel
    {
        public string CallBackUrl { get; set; }
    }
}
