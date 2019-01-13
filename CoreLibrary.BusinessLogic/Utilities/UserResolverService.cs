using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CoreLibrary.BusinessLogic.Utilities
{
    public class UserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetUser()
        {
            return _context.HttpContext?.User?.Identity?.Name;
        }
    }
}
