using System;
using System.Collections.Generic;

namespace Wrhs.Documents
{
    public interface IDocument<TLine> where TLine : IDocumentLine
    {
        string FullNumber { get; set; }

        DateTime IssueDate { get; set; }

        List<TLine> Lines { get; }
    }
}