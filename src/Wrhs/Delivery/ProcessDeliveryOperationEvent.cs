using System;
using System.Collections.Generic;
using Wrhs.Common;

namespace Wrhs.Delivery
{
    public class ProcessDeliveryOperationEvent : ProcessOperationEvent
    {
        public ProcessDeliveryOperationEvent(IEnumerable<Shift> shifts, DateTime createdAt) 
            : base(shifts, createdAt)
        {
        }
    }
}