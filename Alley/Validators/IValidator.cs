using System.ComponentModel.DataAnnotations;
using Alley.Models;

namespace Alley.Validators
{
    internal interface IValidator<T>
    {
        ValidationResult Validate(AlleyMethodModel methodModel);
    }
}