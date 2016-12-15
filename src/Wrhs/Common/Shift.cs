using Wrhs.Products;

namespace Wrhs.Common
{
    public class Shift
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int OperationId { get; set; }

        public decimal Quantity { get; set; }

        public string Location { get; set; }

        public bool Confirmed { get; set; }


        public virtual Operation Operation { get; set; }
        public virtual Product Product { get; set; }
    }
}