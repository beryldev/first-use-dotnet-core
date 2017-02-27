using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Release
{
    public class UpdateReleaseDocumentCommandValidator
        : IValidator<UpdateReleaseDocumentCommand>
    {
        private readonly IValidator<CreateReleaseDocumentCommand> validator;

        public UpdateReleaseDocumentCommandValidator(IValidator<CreateReleaseDocumentCommand> validator)
        {
            this.validator = validator;
        }

        public IEnumerable<ValidationResult> Validate(UpdateReleaseDocumentCommand command)
        {
            return validator.Validate(command as CreateReleaseDocumentCommand);
        }
    }
}