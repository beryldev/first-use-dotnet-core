
using System.Collections.Generic;

namespace Wrhs.Operations
{
    public class OperationState<TDoc>
    {
        public TDoc BaseDocument { get; set; }

        public IEnumerable<Allocation> PendingAllocations { get; set; } = new List<Allocation>();
    }
}
