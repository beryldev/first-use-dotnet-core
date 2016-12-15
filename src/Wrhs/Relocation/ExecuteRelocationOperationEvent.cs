using System;
using Wrhs.Common;

namespace Wrhs.Relocation
{
    public class ExecuteRelocationOperationEvent
        : ExecuteOperationEvent
    {
        public ExecuteRelocationOperationEvent(Operation operation, DateTime executedAt) 
            : base(operation, executedAt)
        {
        }
    }
}