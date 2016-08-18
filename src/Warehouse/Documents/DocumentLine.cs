using System;

namespace Warehouse.Documents
{
    public abstract class DocumentLine
    {
        public virtual string ProductName { get; set; }

        public virtual string ProductCode { get; set; }

        public virtual string EAN { get; set; }

        public virtual string SKU { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual string Remarks { get; set; }
    }
}