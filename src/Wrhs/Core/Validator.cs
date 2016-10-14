using System.Collections.Generic;

namespace Wrhs.Products.Core
{
    public abstract class Validator<TCommand> : IValidator<TCommand>
    {
        public abstract IEnumerable<ValidationResult> Validate(TCommand command);
    }
}