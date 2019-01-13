using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CoreLibrary.Application.Model.Identities
{
    public class ApplicationRole : IdentityRole 
    {
        public ApplicationRole() { }

        public ApplicationRole(string name) : this() => Name = name;
    }
}
