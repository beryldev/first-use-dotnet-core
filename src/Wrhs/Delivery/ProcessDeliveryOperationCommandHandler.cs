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
            IShiftPersist shiftPersist, IOperationService operationSrv) : base(validator, eventBus, shiftPersist, operationSrv)
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

            shiftPersist.Save(shift);

            return new Shift[] { shift };
        }
    }
}