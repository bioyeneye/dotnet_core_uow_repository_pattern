using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoreLibrary.BusinessLogic.Utilities
{
    public class Constants
    {
        public enum UserRoles
        {
            [Description("Super Administrator")]
            SuperAdmin = 1,
            [Description("Admin")]
            Admin,
            Support,
            User
        }
    }
}
