namespace Alley.Validators
{
    internal class ValidationResult
    {
        public bool IsSuccess { get; }
        public bool IsFailed => !IsSuccess;
        public string ErrorMessage { get; }

        public ValidationResult(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}