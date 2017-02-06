using System;
using System.Collections.Generic;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Delivery
{
    public class ProcessDeliveryOperationCommandHandler 
        : ProcessOperationCommandHandler<ProcessDeliveryOperationCommand, ProcessDeliveryOperationEvent>
    {
        public ProcessDeliveryOperationCommandHandler(IValidator<ProcessDeliveryOperationCommand> validator, IEventBus eventBus, 
            IStockService stockService, IOperationService operationSrv) : base(validator, eventBus, stockService, operationSrv)
        {
        }

        protected override ProcessDeliveryOperationEvent CreateEvent(IEnumerable<Shift> shifts, DateTime createdAt)
        {
            return new ProcessDeliveryOperationEvent(shifts, createdAt);
        }

        protected override IEnumerable<Shift> RegisterShifts(ProcessDeliveryOperationCommand command)
        {
            var operation = operationSrv.GetOperationByGuid(command.OperationGuid);
            var shift = new Shift
            {
                OperationId = operation.Id,
                ProductId = command.ProductId,
                Quantity = command.Quantity,
                Location = command.DstLocation
            };

            stockService.Save(shift);

            return new Shift[] { shift };
        }
    }
}