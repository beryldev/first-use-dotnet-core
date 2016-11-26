using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wrhs.Operations;
using Wrhs.Operations.Relocation;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Controllers.Operations
{
    [Route("api/operation/relocation")]
    public class RelocationOperationController
        : OperationController<RelocationOperation, RelocationDocument, RelocationRequest>
    {
        public RelocationOperationController(ICache cache, ILogger<RelocationOperation> logger) 
            : base(cache, logger)
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
            operation.RelocateItem(request.Line.Product, request.From, request.To, request.Quantity);
        }

        protected override OperationState<RelocationDocument> ReadOperationState(RelocationOperation operation)
        {
            var state = new OperationState<RelocationDocument>
            {
                BaseDocument = operation.BaseDocument,
                PendingAllocations = operation.PendingAllocations.ToList()
            };

            return state;
        }

        protected override void OnError(Exception e, RelocationOperation operation)
        {
            logger.LogWarning(e.Message, operation.PendingAllocations.Count);
        }
    }


    public class RelocationRequest
    {
        public RelocationDocumentLine Line { get; set; }

        public decimal Quantity { get; set; }

        public string From { get; set; }

        public string To { get; set; }
    }
}