using System.Collections.Generic;

namespace Wrhs
{
    public interface IStockCache
    {
        IEnumerable<Stock> Read();

        void Refresh(Warehouse warehouse);
    }
}