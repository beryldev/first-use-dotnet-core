using System.Collections.Generic;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Delivery
{
    public class CreateDeliveryDocumentCommandValidator 
        : CommandValidator<CreateDeliveryDocumentCommand>
    {
        private readonly ProductInfoValidator productValidator;

        public CreateDeliveryDocumentCommandValidator(IProductService productSrv)
        {
            this.productValidator = new ProductInfoValidator(productSrv);
        }

        public override IEnumerable<ValidationResult> Validate(CreateDeliveryDocumentCommand command)
        {
            foreach(var line in command.Lines)
                results.AddRange(productValidator.Validate(line));

            return results;
        }
    }
}