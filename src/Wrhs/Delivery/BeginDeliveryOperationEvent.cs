using System;
using Wrhs.Common;

namespace Wrhs.Delivery
{
    public class BeginDeliveryOperationEvent : BeginOperationEvent
    {
        public BeginDeliveryOperationEvent(Operation operation, DateTime createdAt) 
            : base(operation, createdAt)
        {
        }
    }
}