
namespace Wrhs.Documents
{
    public abstract class DocumentLine
    {
        public virtual Product Product { get; set; }

        public virtual string EAN { get; set; }

        public virtual string SKU { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual string Remarks { get; set; }
    }
}