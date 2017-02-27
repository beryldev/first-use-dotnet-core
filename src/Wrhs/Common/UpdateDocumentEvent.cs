using System;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class UpdateDocumentEvent : IEvent
    {
        public UpdateDocumentEvent(int documentId, DateTime updatedAt)
        {
            this.DocumentId = documentId;
            this.UpdatedAt = updatedAt;

        }
        
        public int DocumentId { get; }

        public DateTime UpdatedAt { get; }
    }
}