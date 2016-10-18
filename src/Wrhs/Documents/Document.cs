using System;
using System.Collections.Generic;

namespace Wrhs.Documents
{
    public class Document : IDocument
    {
        public virtual int Id { get; set; }

        public virtual string FullNumber { get; set; }

        public virtual DateTime IssueDate { get; set; }

        public virtual List<DocumentLine> Lines { get; } = new List<DocumentLine>();

    }
}