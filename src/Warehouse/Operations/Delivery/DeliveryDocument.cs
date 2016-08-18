
using System.Collections.Generic;
using Warehouse.Documents;

namespace Warehouse.Operations.Delivery
{
    public class DeliveryDocument : Document
    {
        public new List<DeliveryDocumentLine> Lines { get; set; } = new List<DeliveryDocumentLine>();
    }
}