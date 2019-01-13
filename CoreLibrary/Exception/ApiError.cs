using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Collections.Generic;
using CoreLibrary.Exception;

namespace AspNetCoreSpa.Web.Filters
{

    public class ApiError
    {
        public string Message { get; set; }
        public bool IsError { get; set; }
        public string Detail { get; set; }
        public List<ValidationError> Errors { get; set; }

        public ApiError(string message)
        {
            this.Message = message;
            IsError = true;
        }

        public ApiError(ModelStateDictionary modelState)
        {
            this.IsError = true;
            Message = "Validation Failed";
            Errors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToList();
        }
    }
}