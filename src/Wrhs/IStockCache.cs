using System.Collections.Generic;

namespace Wrhs.Products 
{
    public interface IStockCache
    {
        IEnumerable<Stock> Read();

        void Refresh(Warehouse warehouse);
    }
}