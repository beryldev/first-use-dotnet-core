using System;
using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Documents
{
    public class Document : IDocument, IEntity
    {
        public virtual int Id { get; set; }

        public virtual string FullNumber { get; set; }

        public virtual DateTime IssueDate { get; set; }

        public virtual List<DocumentLine> Lines { get; } = new List<DocumentLine>();

    }
}