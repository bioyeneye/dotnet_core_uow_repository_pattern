using System.Collections.Generic;

namespace CoreLibrary.Exception
{
    public class ApiException : System.Exception
    {
        public int StatusCode { get; set; }

        public List<ValidationError> Errors { get; set; }

        public ApiException(string message, int statusCode = 500, List<ValidationError> errors = null) :
            base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
        public ApiException(System.Exception ex, int statusCode = 500) : base(ex.Message)
        {
            StatusCode = statusCode;
        }
    }
}
