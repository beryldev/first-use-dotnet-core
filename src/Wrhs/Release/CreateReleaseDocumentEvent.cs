using System;
using Wrhs.Common;

namespace Wrhs.Release
{
    public class CreateReleaseDocumentEvent : CreateDocumentEvent
    {
        public CreateReleaseDocumentEvent(Document document, DateTime createdAt) 
            : base(document, createdAt)
        {
        }
    }
}