using Microsoft.Extensions.Logging;
using Wrhs.Operations.Delivery;
using Wrhs.Products;
using Wrhs.WebApp.Controllers.Operations;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Tests
{
    public class DeliveryOperationControllerTests
        : BaseOperationControllerTests<DeliveryOperation, DeliveryDocument, DeliveryDocumentLine, AllocationRequest, DeliveryOperationController>
    {
        protected override DeliveryDocument CreateDocument()
        {
            return new DeliveryDocument(){ FullNumber = DOC_NUMBER };
        }

        protected override DeliveryDocumentLine CreateDocumentLine(string code = "PROD1")
        {
            return new DeliveryDocumentLine
            {
                Product = new Product { Id = 1, Name = "Product 1", Code = code},
                Quantity = 5
            };
        }

        protected override AllocationRequest CreateRequest(DeliveryDocumentLine line, decimal quantity=1)
        {
            return new AllocationRequest
            {
                Line = line,
                Quantity = quantity,
                Location = "LOC-001-01"
            };
        }

        protected override DeliveryOperationController CreateController(ICache cache, ILogger<DeliveryOperation> logger)
        {
            return new DeliveryOperationController(cache, logger);
        } 
    }
}