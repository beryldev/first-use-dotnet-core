using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Delivery
{
    public class UpdateDeliveryDocumentCommandValidator
        : IValidator<UpdateDeliveryDocumentCommand>
    {
        private readonly CreateDeliveryDocumentCommandValidator validator;

        public UpdateDeliveryDocumentCommandValidator(CreateDeliveryDocumentCommandValidator validator)
        {
            this.validator = validator;
        }

        public IEnumerable<ValidationResult> Validate(UpdateDeliveryDocumentCommand command)
        {
            return validator.Validate(command as CreateDeliveryDocumentCommand);
        }
    }
}