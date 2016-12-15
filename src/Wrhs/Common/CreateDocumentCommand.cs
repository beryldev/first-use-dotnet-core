using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public abstract class CreateDocumentCommand : ICommand
    {
        public IEnumerable<DocumentLine> Lines { get; set; }

        public class DocumentLine : IValidableProductInfo
        {
            public int ProductId { get; set; }
            public decimal Quantity { get; set; }
            public string SrcLocation { get; set; }
            public string DstLocation { get; set; }
        }
    }
}