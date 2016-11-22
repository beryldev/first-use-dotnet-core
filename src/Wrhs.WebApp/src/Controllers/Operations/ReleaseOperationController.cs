using System;
using Wrhs.Operations;
using Wrhs.Operations.Release;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Controllers.Operations
{
    public class ReleaseOperationController
        : OperationController<ReleaseOperation, ReleaseDocument, ReleaseRequest>
    {
        public ReleaseOperationController(ICache cache) : base(cache)
        {
        }

        protected override ReleaseOperation CreateOperation(OperationState<ReleaseDocument> state)
        {
            return new ReleaseOperation(state);
        }

        protected override ReleaseOperation CreateOperation(ReleaseDocument baseDocument)
        {
            var operation = new ReleaseOperation();
            operation.SetBaseDocument(baseDocument);

            return operation;
        }

        protected override void DoStep(ReleaseOperation operation, ReleaseRequest request)
        {
            operation.ReleaseItem(request.Line.Product, request.Location, request.Quantity);
        }

        protected override OperationState<ReleaseDocument> ReadOperationState(ReleaseOperation operation)
        {
            var state = new OperationState<ReleaseDocument>
            {
                BaseDocument = operation.BaseDocument,
                PendingAllocations = operation.PendingAllocations
            };

            return state;
        }
    }


    public class ReleaseRequest
    {
        public ReleaseDocumentLine Line { get; set; }

        public decimal Quantity { get; set; }

        public string Location { get; set; }
    }
}