using System;
using System.Collections.Generic;

namespace Warehouse.Documents
{
    public abstract class Document
    {
        public virtual int Id { get; set; }

        public virtual string FullNumber { get; set; }

        public virtual DateTime IssueDate { get; set; }

        public virtual List<DocumentLine> Lines { get; set; }
    }
}