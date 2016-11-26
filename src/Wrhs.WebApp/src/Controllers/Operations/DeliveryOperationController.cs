using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wrhs.Operations;
using Wrhs.Operations.Delivery;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Controllers.Operations
{
    [Route("api/operation/delivery")]
    public class DeliveryOperationController 
        : OperationController<DeliveryOperation, DeliveryDocument, AllocationRequest>
    {
    
        public DeliveryOperationController(ICache cache, ILogger<DeliveryOperation> logger)
            : base(cache, logger) { }

        protected override DeliveryOperation CreateOperation(DeliveryDocument baseDocument)
        {
            
            var operation = new DeliveryOperation();
            operation.SetBaseDocument(baseDocument);
            return operation;
        }

        protected override DeliveryOperation CreateOperation(OperationState<DeliveryDocument> state)
        {
            return new DeliveryOperation(state);
        }

        protected override void DoStep(DeliveryOperation operation, AllocationRequest request)
        {
            operation.AllocateItem(request.Line, request.Quantity, request.Location);
        }

        protected override OperationState<DeliveryDocument> ReadOperationState(DeliveryOperation operation)
        {
           return operation.ReadState();
        }       
    }


    public class AllocationRequest
    {
        public DeliveryDocumentLine Line { get; set; }

        public string Location { get; set; }

        public decimal Quantity { get; set; } = 0;
    }
}