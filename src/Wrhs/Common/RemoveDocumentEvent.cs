using System;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class RemoveDocumentEvent : IEvent
    {
        public RemoveDocumentEvent(Document removedDocument, DateTime removedAt)
        {
            this.RemovedDocument = removedDocument;
            this.RemovedAt = removedAt;

        }
        public Document RemovedDocument { get; }
        public DateTime RemovedAt { get; }
    }
}