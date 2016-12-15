using System;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class ExecuteOperationEvent : IEvent
    {
        public ExecuteOperationEvent(Operation operation, DateTime executedAt)
        {
            this.Operation = operation;
            this.ExecutedAt = executedAt;
        }

        public Operation Operation { get; }
        public DateTime ExecutedAt { get; }
    }
}