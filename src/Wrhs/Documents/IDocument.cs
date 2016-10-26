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


    public interface ISearchableDocument
    {
        string FullNumber { get; set; }

        DateTime IssueDate { get; set; }
    }


    public interface INumerableDocument
    {
        string FullNumber { get; set; }

        int Number { get; set; }
    }
}