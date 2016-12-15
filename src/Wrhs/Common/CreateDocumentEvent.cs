using System;
using Wrhs.Core;

namespace Wrhs.Common
{
    public abstract class CreateDocumentEvent: IEvent
    {
        protected CreateDocumentEvent(Document document, DateTime createdAt)
        {
            this.Document = document;
            this.CreatedAt = createdAt;

        }
        
        public Document Document { get; }
        public DateTime CreatedAt { get; }
    }
}