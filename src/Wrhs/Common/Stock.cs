using Wrhs.Products;

namespace Wrhs.Common
{
    public class Stock
    {
        public int ProductId { get; set; }
        public string Location { get; set; }
        public decimal Quantity { get; set; }

        public virtual Product Product { get; set; }
    }
}