
using FluentValidation.Results;
using TheBeans.Application.Common.Responses;

namespace TheBeans.Application.Common.Exceptions
{

    public class AppValidationException : ApplicationException
    {
        public List<string> ValidationErrors { get; set; }
        public AppValidationException(BaseResponse baseResponse)
        {
            ValidationErrors = baseResponse.ValidationErrors;
        }
    }
}