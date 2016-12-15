using System;
using Wrhs.Common;

namespace Wrhs.Release
{
    public class ExecuteReleaseOperationEvent : ExecuteOperationEvent
    {
        public ExecuteReleaseOperationEvent(Operation operation, DateTime executedAt) 
            : base(operation, executedAt)
        {
        }
    }
}