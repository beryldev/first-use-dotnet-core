using System;
using System.Collections.Generic;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Release
{
    public class ProcessReleaseOperationCommandHandler
        : ProcessOperationCommandHandler<ProcessReleaseOperationCommand, ProcessReleaseOperationEvent>
    {
        public ProcessReleaseOperationCommandHandler(IValidator<ProcessReleaseOperationCommand> validator, 
            IEventBus eventBus, IShiftPersist shiftPersist, IOperationService operationSrv) 
            : base(validator, eventBus, shiftPersist, operationSrv)
        {
        }

        protected override ProcessReleaseOperationEvent CreateEvent(IEnumerable<Shift> shifts, DateTime createdAt)
        {
            return new ProcessReleaseOperationEvent(shifts, createdAt);
        }

        protected override IEnumerable<Shift> RegisterShifts(ProcessReleaseOperationCommand command)
        {
            var shift = new Shift
            {
                ProductId = command.ProductId,
                Quantity = command.Quantity * (-1),
                Location = command.SrcLocation,
                OperationId = GetOperationId(command.OperationGuid)
            };

            shiftPersist.Save(shift);

            return new Shift[]{ shift };
        }
    }
}