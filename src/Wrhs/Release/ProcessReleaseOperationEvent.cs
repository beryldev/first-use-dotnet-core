using System;
using System.Collections.Generic;
using Wrhs.Common;

namespace Wrhs.Release
{
    public class ProcessReleaseOperationEvent : ProcessOperationEvent
    {
        public ProcessReleaseOperationEvent(IEnumerable<Shift> shifts, DateTime createdAt) 
            : base(shifts, createdAt)
        {
        }
    }
}