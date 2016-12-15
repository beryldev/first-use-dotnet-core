
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Services
{
    public interface IProductService
    {
        Product GetProductById(int id);

        Product GetProductByName(string name);

        Product GetProductByCode(string code);

        bool CheckProductExists(int id);

        bool CheckProductExistsByName(string name);

        bool CheckProductExistsByCode(string code);

        ResultPage<Product> Get();

        ResultPage<Product> Get(int page);

        ResultPage<Product> Get(int page, int pageSize);

        ResultPage<Product> FilterProducts(Dictionary<string, object> filter);

        ResultPage<Product> FilterProducts(Dictionary<string, object> filter, int page, int pageSize);
    }
}