using System;
using Wrhs.Common;

namespace Wrhs.Delivery
{
    public class ExecuteDeliveryOperationEvent : ExecuteOperationEvent
    {
        public ExecuteDeliveryOperationEvent(Operation operation, DateTime executedAt) 
            : base(operation, executedAt)
        {
        }
    }
}