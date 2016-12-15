using System.Collections.Generic;

namespace Wrhs.Core
{
    public interface IValidator<T>
    {
         IEnumerable<ValidationResult> Validate(T command);
    }
}