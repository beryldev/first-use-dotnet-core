using System;
using Warehouse.Documents;

namespace Warehouse.Operations
{
    public class Allocation
    {
        public string ProductCode { get; set; }
        
        public string Location { get; set; }

        public decimal Quantity { get; set; }
    }
}