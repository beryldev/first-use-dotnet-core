using Microsoft.AspNetCore.Mvc;
using Wrhs.Operations;
using Wrhs.Operations.Relocation;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Controllers.Operations
{
    [Route("api/operation/relocation")]
    public class RelocationOperationController
        : OperationController<RelocationOperation, RelocationDocument, RelocationRequest>
    {
        public RelocationOperationController(ICache cache) : base(cache)
        {
        }

        protected override RelocationOperation CreateOperation(OperationState<RelocationDocument> state)
        {
            return new RelocationOperation(state);
        }

        protected override RelocationOperation CreateOperation(RelocationDocument baseDocument)
        {
            var operation = new RelocationOperation();
            operation.SetBaseDocument(baseDocument);

            return operation;
        }

        protected override void DoStep(RelocationOperation operation, RelocationRequest request)
        {
            operation.RelocateItem(request.Line.Product, request.Line.From, request.Line.To, request.Quantity);
        }

        protected override OperationState<RelocationDocument> ReadOperationState(RelocationOperation operation)
        {
            var state = new OperationState<RelocationDocument>
            {
                BaseDocument = operation.BaseDocument,
                PendingAllocations = operation.PendingAllocations
            };

            return state;
        }
    }


    public class RelocationRequest
    {
        public RelocationDocumentLine Line { get; set; }

        public decimal Quantity { get; set; }
    }
}