using Alley.Core.Models;

namespace Alley.Core.Validators
{
    internal interface IValidator<T>
    {
        Result Validate(T validationCandidate);
    }
}