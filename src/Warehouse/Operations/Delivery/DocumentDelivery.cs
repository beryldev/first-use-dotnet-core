using System;
using System.Collections.Generic;

namespace Warehouse.Operations.Delivery
{
    public class DocumentDelivery : BaseDocument
    {
        List<DocumentDeliveryLine> Lines { get; set; } = new List<DocumentDeliveryLine>();
    }

    public class DocumentDeliveryLine : BaseDocumentLine
    {
        public string DestinationAddress { get; set; }
    }
}