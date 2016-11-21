using System;
using Wrhs.Operations.Relocation;
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
            throw new NotImplementedException();
        }

        protected override RelocationDocument CreateDocument()
        {
            throw new NotImplementedException();
        }

        protected override RelocationDocumentLine CreateDocumentLine(string code = "PROD1")
        {
            throw new NotImplementedException();
        }

        protected override RelocationRequest CreateRequest(RelocationDocumentLine line, decimal quantity = 1)
        {
            throw new NotImplementedException();
        }
    }
}