using System.Collections.Generic;

namespace Wrhs.Core
{
    public abstract class CommandValidator<TCommand> : IValidator<TCommand>
        where TCommand : ICommand
    {
        protected readonly List<ValidationResult> results = new List<ValidationResult>();

        public abstract IEnumerable<ValidationResult> Validate(TCommand command);

        protected void AddValidationResult(string field, string message)
        {
            results.Add(new ValidationResult(field ,message));
        }
    }
}