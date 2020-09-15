using Alley.Core.Models;
using Alley.Core.Utilities;

namespace Alley.Core.Validators
{
    internal class MethodModelValidator : IValidator<AlleyMethodModel>
    {
        private bool _isSuccess;
        private string _errorMessage;
        
        public Result Validate(AlleyMethodModel validationCandidate)
        {
            ResetResult();
            ValidatePackageName(validationCandidate.PackageName);
            ValidateServiceName(validationCandidate.ServiceName);
            ValidateMethodName(validationCandidate.MethodName);
            return new Result(_isSuccess, _errorMessage);
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