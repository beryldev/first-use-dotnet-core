using System;
using Wrhs.Common;

namespace Wrhs.Release
{
    public class BeginReleaseOperationEvent : BeginOperationEvent
    {
        public BeginReleaseOperationEvent(Operation operation, DateTime createdAt) 
            : base(operation, createdAt)
        {
        }
    }
}