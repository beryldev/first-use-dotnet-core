using System;
using System.Collections.Generic;

namespace Wrhs.Common
{
    public class Document
    {
        public int Id { get; set; }
        public DocumentType Type { get; set; }
        public DocumentState State { get; set; }
        public string FullNumber { get; set; }
        public int Number { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime IssueDate { get; set; }
        public string Remarks { get; set; }

        public virtual List<DocumentLine> Lines { get; set; }
    }
}