using System;
using System.Collections.Generic;
using Wrhs.Common;

namespace Wrhs.Relocation
{
    public class ProcessRelocationOperationEvent
        : ProcessOperationEvent
    {
        public ProcessRelocationOperationEvent(IEnumerable<Shift> shifts, DateTime createdAt) 
            : base(shifts, createdAt)
        {
        }
    }
}