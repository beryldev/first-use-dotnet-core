using System;
using Wrhs.Common;

namespace Wrhs.Relocation
{
    public class BeginRelocationOperationEvent : BeginOperationEvent
    {
        public BeginRelocationOperationEvent(Operation operation, DateTime createdAt) : base(operation, createdAt)
        {
        }
    }
}