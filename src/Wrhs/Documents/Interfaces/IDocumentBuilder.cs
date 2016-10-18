using System;
using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Documents.Interfaces
{
    public interface IDocumentBuilder<IDocument, IDocumentLine>
    {
        event EventHandler<IEnumerable<ValidationResult>> OnAddLineFail;

        event EventHandler<IEnumerable<ValidationResult>> OnUpdateLineFail;
        
        IEnumerable<IDocumentLine> Lines { get; }

        IDocument Build();  

        void AddLine(DocumentBuilderAddLineCommand command);

        void RemoveLine(IDocumentLine line);

        void UpdateLine(IDocumentLine line);
    }
}