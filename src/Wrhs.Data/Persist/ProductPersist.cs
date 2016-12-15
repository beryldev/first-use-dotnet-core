using Wrhs.Products;

namespace Wrhs.Data.Persist
{
    public class ProductPersist : BaseData, IProductPersist
    {
        public ProductPersist(WrhsContext context) : base(context)
        {
        }

        public int Save(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return product.Id;
        }

        public void Update(Product product)
        {
            context.SaveChanges();
        }
    }
}