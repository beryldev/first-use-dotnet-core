
using Wrhs.Products;

namespace Wrhs.Documents
{
    public class DocumentLine : IDocumentLine
    {
        public virtual int Id { get; set; } = 0;
        
        public virtual Product Product { get; set; }

        public virtual string EAN { get; set; }

        public virtual string SKU { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual string Remarks { get; set; }
    }
}