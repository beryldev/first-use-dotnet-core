using System;
using Wrhs.Core;

namespace Wrhs.Delivery
{
    public class UpdateDeliveryDocumentEvent : IEvent
    {
        public UpdateDeliveryDocumentEvent(int documentId, DateTime updatedAt)
        {
            this.DocumentId = documentId;
            this.UpdatedAt = updatedAt;

        }
        
        public int DocumentId { get; }

        public DateTime UpdatedAt { get; }
    }
}