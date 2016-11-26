using Microsoft.Extensions.Logging;
using Wrhs.Operations.Release;
using Wrhs.Products;
using Wrhs.WebApp.Controllers.Operations;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Tests
{
    public class ReleaseOperationControllerTests
        : BaseOperationControllerTests<ReleaseOperation, ReleaseDocument, ReleaseDocumentLine,
            ReleaseRequest, ReleaseOperationController>
    {
        protected override ReleaseOperationController CreateController(ICache cache, ILogger<ReleaseOperation> logger)
        {
            return new ReleaseOperationController(cache, logger);
        }

        protected override ReleaseDocument CreateDocument()
        {
            return new ReleaseDocument() { FullNumber = DOC_NUMBER };
        }

        protected override ReleaseDocumentLine CreateDocumentLine(string code = "PROD1")
        {
            return new ReleaseDocumentLine
            {
                Product = new Product { Id = 1, Name = "Product 1", Code = code},
                Quantity = 5,
                Location = "LOC-001-01"
            };
        }

        protected override ReleaseRequest CreateRequest(ReleaseDocumentLine line, decimal quantity = 1)
        {
            return new ReleaseRequest
            {
                Line = line,
                Quantity = quantity,
                Location = "LOC-001-01"
            };
        }
    }
}