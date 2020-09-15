using Alley.Core.Models;
using Alley.Core.Serialization;
using Alley.Core.Utilities;
using Alley.Core.Validators;
using Grpc.Core;
using Serilog;

namespace Alley.Core.Factories
{
    internal class MethodFactory
    {
        private readonly MethodModelValidator _methodModelValidator = new MethodModelValidator();
        
        public Result<Method<IAlleyMessageModel, IAlleyMessageModel>> Create(AlleyMethodModel methodModel)
        {
            var modelValidationResult = _methodModelValidator.Validate(methodModel);
            if (modelValidationResult.IsFailure)
            {
                Log.Error(LogMessageFactory.Create(modelValidationResult.ErrorMessage));
                return Result<Method<IAlleyMessageModel, IAlleyMessageModel>>.Failure(modelValidationResult.ErrorMessage);
            }
            var serviceFullName = FormatServiceFullName(methodModel.PackageName, methodModel.ServiceName);

            var method = CreateMethod(methodModel.MethodType,serviceFullName,methodModel.MethodName);
            return Result<Method<IAlleyMessageModel, IAlleyMessageModel>>.Success(method);
        }

        private Method<IAlleyMessageModel, IAlleyMessageModel> CreateMethod(MethodType methodType, string serviceFullName, string methodName)
        {
            return new Method<IAlleyMessageModel, IAlleyMessageModel>(
                methodType,
                serviceFullName,
                methodName,
                AlleyMessageSerializer.AlleyMessageMarshaller,
                AlleyMessageSerializer.AlleyMessageMarshaller);
        }
        
        private string FormatServiceFullName(string package, string serviceName)
        {
            return $"{package}.{serviceName}";
        }
    }
}
