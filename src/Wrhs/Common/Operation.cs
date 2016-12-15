using System.Collections.Generic;

namespace Wrhs.Common
{
    public class Operation
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string OperationGuid { get; set; }
        public OperationType Type { get; set; }
        public OperationStatus Status { get; set; }


        public virtual ICollection<Shift> Shifts { get; set; }
        public virtual Document Document { get; set; }
    }
}