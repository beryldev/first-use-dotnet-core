using System.Collections.Generic;

namespace Wrhs.Common
{
    public class Document
    {
        public int Id { get; set; }
        public DocumentType Type { get; set; }
        public DocumentState State { get; set; }

        public virtual List<DocumentLine> Lines { get; set; }
    }
}