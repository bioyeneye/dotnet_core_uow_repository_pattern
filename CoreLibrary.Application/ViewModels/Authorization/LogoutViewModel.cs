using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoreLibrary.Application.ViewModels.Authorization
{
    public class LogoutViewModel
    {
        [BindNever]
        public string RequestId { get; set; }
    }
}
