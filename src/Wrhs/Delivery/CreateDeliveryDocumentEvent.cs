using System;
using Wrhs.Common;

namespace Wrhs.Delivery
{
    public class CreateDeliveryDocumentEvent : CreateDocumentEvent
    {
        public CreateDeliveryDocumentEvent(Document document, DateTime createdAt) 
            : base(document, createdAt)
        {
        }
    }
}