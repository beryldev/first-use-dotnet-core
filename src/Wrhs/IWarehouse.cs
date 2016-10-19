using System.Collections.Generic;
using Wrhs.Operations;

namespace Wrhs
{
    public interface IWarehouse
    {
        void ProcessOperation(IOperation operation);

        List<Stock> CalculateStocks();

        List<Stock> CalculateStocks(string productCode);

        List<Stock> ReadStocks();

        List<Stock> ReadStocksByProductCode(string productCode);

        List<Stock> ReadStocksByLocation(string location);
    }
}