using System.Collections.Generic;

namespace Wrhs.Core
{
    public interface IValidator<TCommand>
    {
        IEnumerable<ValidationResult> Validate(TCommand command);
    }
}