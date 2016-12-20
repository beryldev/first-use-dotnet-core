using Wrhs.Products;

namespace Wrhs.Data.Persist
{
    public class ProductPersist : BaseData, IProductPersist
    {
        public ProductPersist(WrhsContext context) : base(context)
        {
        }

        public void Delete(Product product)
        {
            if(product == null)
                return;
                
            context.Products.Remove(product);
            context.SaveChanges();
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