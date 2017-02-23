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

        ResultPage<Product> FilterProducts(ProductFilter filter);

        ResultPage<Product> FilterProducts(ProductFilter filter, int page, int pageSize);
    
        int Save(Product product);

        void Update(Product product);

        void Delete(Product Product);
    }


    public class ProductFilter
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Ean { get; set; } = string.Empty;
    }
}