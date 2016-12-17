using System.Collections.Generic;

namespace Wrhs.Common
{
    public interface IStockService
    {
         Stock GetStockAtLocation(int productId, string location);

         IEnumerable<Stock> GetProductStock(int productId);
    }
}