using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Delivery
{
    public class UpdateDeliveryDocumentCommandValidator : IValidator<UpdateDeliveryDocumentCommand>
    {
        private readonly IValidator<CreateDeliveryDocumentCommand> validator;

        public UpdateDeliveryDocumentCommandValidator(IValidator<CreateDeliveryDocumentCommand> validator)
        {
            this.validator = validator;
        }

        public IEnumerable<ValidationResult> Validate(UpdateDeliveryDocumentCommand command)
        {
            return validator.Validate(command as CreateDeliveryDocumentCommand);
        }
    }
}