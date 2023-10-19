using FluentValidation.Results;

namespace EngineeringThesisAPI.Models.ValidationErrorModel
{
    public class ErrorModel
    {
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
    }

    public class ValidationErrorModel<T>
    {
        public List<ErrorModel> Errors { get; set; }

        public ValidationErrorModel(ValidationResult validationResult)
        {
            Errors = new List<ErrorModel>();

            Errors = validationResult.Errors.Select(error => new ErrorModel
            {
                ErrorMessage = error.ErrorMessage,
                ErrorCode= error.ErrorCode,
            }).ToList();
        }
    }
}
