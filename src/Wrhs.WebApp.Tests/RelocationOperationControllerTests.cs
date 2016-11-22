using System;
using Wrhs.Operations.Relocation;
using Wrhs.Products;
using Wrhs.WebApp.Controllers.Operations;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Tests
{
    public class RelocationOperationControllerTests
        : BaseOperationControllerTests<RelocationOperation, RelocationDocument,
            RelocationDocumentLine, RelocationRequest, RelocationOperationController>
    {
        protected override RelocationOperationController CreateController(ICache cache)
        {
            return new RelocationOperationController(cache);
        }

        protected override RelocationDocument CreateDocument()
        {
            return new RelocationDocument() { FullNumber = DOC_NUMBER };
        }

        protected override RelocationDocumentLine CreateDocumentLine(string code = "PROD1")
        {
             return new RelocationDocumentLine
            {
                Product = new Product { Id = 1, Name = "Product 1", Code = code},
                Quantity = 5,
                From = "LOC-001-01",
                To = "LOC-001-02"
            };
        }

        protected override RelocationRequest CreateRequest(RelocationDocumentLine line, decimal quantity = 1)
        {
            return new RelocationRequest
            {
                Line = line,
                Quantity = quantity,
                From = "LOC-001-01",
                To = "LOC-001-02"
            };
        }
    }
}