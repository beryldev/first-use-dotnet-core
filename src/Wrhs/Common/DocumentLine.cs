using Wrhs.Products;

namespace Wrhs.Common
{
    public class DocumentLine
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string SrcLocation { get; set; } = string.Empty;
        public string DstLocation { get; set; } = string.Empty;

        public virtual Product Product { get; set; }
        public virtual Document Document { get; set; }
    }
}