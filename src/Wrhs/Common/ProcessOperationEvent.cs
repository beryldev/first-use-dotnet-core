using System;
using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class ProcessOperationEvent : IEvent
    {
        public ProcessOperationEvent(IEnumerable<Shift> shifts, DateTime createdAt)
        {
            this.Shifts = shifts;
            this.CreatedAt = createdAt;

        }
        public IEnumerable<Shift> Shifts { get; }

        public DateTime CreatedAt { get; }
    }
}