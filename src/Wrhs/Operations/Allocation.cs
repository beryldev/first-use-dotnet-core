using System;
using Wrhs.Documents;

namespace Wrhs.Operations
{
    public class Allocation : IEntity
    {
        public int Id { get; set; }
        
        public string ProductCode { get; set; }
        
        public string Location { get; set; }

        public decimal Quantity { get; set; }
    }
}