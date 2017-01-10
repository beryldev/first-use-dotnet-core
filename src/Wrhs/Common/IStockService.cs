using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public interface IStockService
    {
         Stock GetStockAtLocation(int productId, string location);

         IEnumerable<Stock> GetProductStock(int productId);

         ResultPage<Stock> GetStocks(int page, int pageSize);
    }
}