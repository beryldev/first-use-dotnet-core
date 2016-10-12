
using System.Collections.Generic;
using System.Linq;

namespace Wrhs.Core
{
    public class ValidationCommandHandlerDecorator<T> : ICommandHandler<T>
    {
        ICommandHandler<T> handler;

        IValidator<T> validator;

        IEnumerable<ValidationResult> validationResults = new List<ValidationResult>();

        public IEnumerable<ValidationResult> ValidationResults { get { return validationResults.ToList(); }  }

        public ValidationCommandHandlerDecorator(ICommandHandler<T> handler, IValidator<T> validator)
        {
            this.handler = handler;
            this.validator = validator;
        }

        public void Handle(T command)
        {
            validationResults = validator.Validate(command);
            if(validationResults.Count() == 0)
                handler.Handle(command);
        }
    }
}