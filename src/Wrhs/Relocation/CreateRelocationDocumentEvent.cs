using System;
using Wrhs.Common;

namespace Wrhs.Relocation
{
    public class CreateRelocationDocumentEvent : CreateDocumentEvent
    {
        public CreateRelocationDocumentEvent(Document document, DateTime createdAt) 
            : base(document, createdAt)
        {
        }
    }
}