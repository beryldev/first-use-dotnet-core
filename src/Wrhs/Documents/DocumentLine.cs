
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Documents
{
    public class DocumentLine : IDocumentLine, IEntity
    {
        public virtual int Id { get; set; } = 0;

        public virtual int Lp { get; set; }
        
        public virtual Product Product { get; set; }

        public virtual string EAN { get; set; }

        public virtual string SKU { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual string Remarks { get; set; }
    }
}