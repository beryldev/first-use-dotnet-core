using System.Collections.Generic;
using Warehouse.Documents;

namespace Warehouse.Orders
{
    public class Order : Document
    {
        public new List<OrderLine> Lines { get; set; } = new List<OrderLine>();
    }
}