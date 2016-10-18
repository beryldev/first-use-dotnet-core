using System;
using System.Collections.Generic;

namespace Wrhs.Documents
{
    public interface IDocument
    {
        string FullNumber { get; set; }

        DateTime IssueDate { get; set; }

        List<DocumentLine> Lines { get; }
    }
}