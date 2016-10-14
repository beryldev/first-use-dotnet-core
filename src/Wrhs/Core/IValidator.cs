using System.Collections.Generic;

namespace Wrhs.Products.Core
{
    public interface IValidator<TCommand>
    {
        IEnumerable<ValidationResult> Validate(TCommand command);
    }
}