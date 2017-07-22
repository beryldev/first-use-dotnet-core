using System.Collections.Generic;

namespace Wrhs.Core
{
    public interface IValidator<T> : IValidator
    {
         IEnumerable<ValidationResult> Validate(T command);
    }

    public interface IValidator
    {

    }
}