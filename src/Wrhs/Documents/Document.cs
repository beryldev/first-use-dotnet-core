using System;
using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Documents
{
    public class Document<TLine> : IDocument<TLine>, IEntity
        where TLine : IDocumentLine
    {
        public virtual int Id { get; set; }

        public virtual string FullNumber { get; set; }

        public virtual DateTime IssueDate { get; set; }

        public virtual List<TLine> Lines { get; } = new List<TLine>();

    }
}