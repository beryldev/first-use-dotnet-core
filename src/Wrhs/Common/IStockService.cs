namespace Wrhs.Common
{
    public interface IStockService
    {
         Stock GetStockAtLocation(int productId, string location);
    }
}