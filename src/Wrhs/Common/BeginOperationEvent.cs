using System;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class BeginOperationEvent : IEvent
    {
        public BeginOperationEvent(Operation operation, DateTime createdAt)
        {
            this.Operation = operation;
            this.CreatedAt = createdAt;

        }
        public Operation Operation { get; }

        public DateTime CreatedAt { get; }
    }
}