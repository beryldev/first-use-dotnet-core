using System;
using System.Collections.Generic;

namespace Wrhs.Documents
{
    public abstract class DocumentLine
    {
        public virtual string ProductName { get; set; }

        public virtual string ProductCode { get; set; } = String.Empty;

        public virtual string EAN { get; set; }

        public virtual string SKU { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual string Remarks { get; set; }
    }
}