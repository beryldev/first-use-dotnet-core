
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Operations
{
    public class Allocation : IEntity
    {
        public int Id { get; set; }
        
        public virtual Product Product { get; set; }
        
        public string Location { get; set; }

        public decimal Quantity { get; set; }
    }
}