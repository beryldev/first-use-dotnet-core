using System;
using System.Collections.Generic;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Relocation
{
    public class ProcessRelocationOperationCommandHandler
        : ProcessOperationCommandHandler<ProcessRelocationOperationCommand, ProcessRelocationOperationEvent>
    {
        public ProcessRelocationOperationCommandHandler(IValidator<ProcessRelocationOperationCommand> validator,
            IEventBus eventBus, IShiftPersist shiftPersist, IOperationService operationSrv) 
            : base(validator, eventBus, shiftPersist, operationSrv)
        {
        }

        protected override ProcessRelocationOperationEvent CreateEvent(IEnumerable<Shift> shifts, DateTime createdAt)
        {
            return new ProcessRelocationOperationEvent(shifts, createdAt);
        }

        protected override IEnumerable<Shift> RegisterShifts(ProcessRelocationOperationCommand command)
        {
            var operationId = GetOperationId(command.OperationGuid);
            var shiftOut = new Shift
            {
                ProductId = command.ProductId,
                Location = command.SrcLocation,
                Quantity = command.Quantity * (-1),
                OperationId = operationId
            };

            var shiftIn = new Shift
            {
                ProductId = command.ProductId,
                Location = command.DstLocation,
                Quantity = command.Quantity,
                OperationId = operationId
            };

            shiftPersist.Save(shiftOut);
            shiftPersist.Save(shiftIn);
            
            return new Shift[]{ shiftOut, shiftIn};
        }
    }
}