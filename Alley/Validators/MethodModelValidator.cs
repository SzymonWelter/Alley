using Alley.Models;
using Alley.Utilities;

namespace Alley.Validators
{
    internal class MethodModelValidator : IValidator<AlleyMethodModel>
    {
        private bool _isSuccess;
        private string _errorMessage;
        
        public ValidationResult Validate(AlleyMethodModel model)
        {
            ResetResult();
            ValidatePackageName(model.PackageName);
            ValidateServiceName(model.ServiceName);
            ValidateMethodName(model.MethodName);
            return new ValidationResult(_isSuccess, _errorMessage);
        }

        private void ResetResult()
        {
            _isSuccess = true;
            _errorMessage = string.Empty;
        }

        private void ValidateMethodName(string methodName)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                HandleUnsuccessfulValidation(ValidationMessages.MethodNameIsNullOrEmpty);
            }
        }

        private void ValidatePackageName(string packageName)
        {
            if (string.IsNullOrEmpty(packageName))
            {
                HandleUnsuccessfulValidation(ValidationMessages.PackageNameIsNullOrEmpty);
            }
        }
        
        private void ValidateServiceName(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                HandleUnsuccessfulValidation(ValidationMessages.ServiceNameIsNullOrEmpty);
            }
        }
        
        private void HandleUnsuccessfulValidation(string validationMessage)
        {
            _isSuccess = false;
            AddMessage(validationMessage);
        }

        private void AddMessage(string message)
        {
            _errorMessage += $"\n{message}";
        }
    }
}