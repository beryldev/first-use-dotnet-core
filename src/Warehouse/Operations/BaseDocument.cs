using System;
using System.Collections.Generic;

namespace Warehouse.Operations
{
    public class BaseDocument : IOperationDocument
    {
        public DateTime OperationDate { get; set; }
        
    }


    public class BaseDocumentLine : IOperationDocumentLine
    {
        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public string EAN { get; set; }

        public string SKU { get; set; }

        public decimal Quantity { get; set; }

        public string Remarks { get; set; }
    }
}