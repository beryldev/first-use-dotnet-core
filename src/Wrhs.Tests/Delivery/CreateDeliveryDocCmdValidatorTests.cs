using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Delivery;
using Wrhs.Services;
using Xunit;

namespace Wrhs.Tests.Delivery
{
    public class CreateDeliveryDocCmdValidatorTests
     : CreateDocumentCommandValidatorTestsBase<CreateDeliveryDocumentCommand>
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void ShouldReturnErrorWhenInvalidDstLocation(string dstLocation)
        {
            var cmd = CreateCommand();
            cmd.Lines = new List<CreateDeliveryDocumentCommand.DocumentLine>
            {
                new CreateDeliveryDocumentCommand.DocumentLine
                {
                    ProductId = 1,
                    Quantity = 10,
                    DstLocation = dstLocation
                }  
            };

            var results = validator.Validate(cmd);

            AssertSingleError(results, "DstLocation");
        }

        protected override CreateDeliveryDocumentCommand CreateCommand()
        {
            return new CreateDeliveryDocumentCommand();
        }

        protected override IValidator<CreateDeliveryDocumentCommand> CreateValidator(IProductService productSrv)
        {
            return new CreateDeliveryDocumentCommandValidator(productSrv);
        }
    }
}