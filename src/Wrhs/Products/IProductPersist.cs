namespace Wrhs.Products
{
    public interface IProductPersist
    {
         int Save(Product product);

         void Update(Product product);

         void Delete(Product Product);
    }
}