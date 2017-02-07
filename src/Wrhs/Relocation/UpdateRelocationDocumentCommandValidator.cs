using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Relocation
{
    public class UpdateRelocationDocumentCommandValidator
        : IValidator<UpdateRelocationDocumentCommand>
    {
        private readonly IValidator<CreateRelocationDocumentCommand> validator;

        public UpdateRelocationDocumentCommandValidator(IValidator<CreateRelocationDocumentCommand> validator)
        {
            this.validator = validator;
        }

        public IEnumerable<ValidationResult> Validate(UpdateRelocationDocumentCommand command)
        {
            return validator.Validate(command as CreateRelocationDocumentCommand);
        }
    }
}