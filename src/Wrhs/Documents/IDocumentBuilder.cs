using System;
using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Documents
{
    public interface IDocumentBuilder<TDocument, TLine> 
        where TDocument : IDocument 
        where TLine :IDocumentLine
    {
        event EventHandler<IEnumerable<ValidationResult>> OnAddLineFail;

        event EventHandler<IEnumerable<ValidationResult>> OnUpdateLineFail;

        IEnumerable<TLine> Lines { get; } 

        TDocument Build();

        void AddLine(DocumentBuilderAddLineCommand command);

        void RemoveLine(TLine line);

        void UpdateLine(TLine line);
    }
}