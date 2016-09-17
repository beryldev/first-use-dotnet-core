using System;
using System.Collections.Generic;
using Wrhs.Operations.Delivery;

namespace Wrhs
{
    public class Warehouse
    {
        public void ProcessOperation(DeliveryOperation operation)
        {
            operation.Perform();
        }

        public List<Stock> CalculateStocks()
        {
            return new List<Stock>();
        }
    }
}