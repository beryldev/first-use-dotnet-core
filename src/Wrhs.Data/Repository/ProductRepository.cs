using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Data.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        WrhsContext context;

        public ProductRepository(WrhsContext context)
        {
            this.context = context;
        }

        public IEnumerable<Product> Get()
        {
            return context.Products;
        }

        public Product GetById(int id)
        {
            return context.Products
                .Where(prod=>prod.Id == id)
                .FirstOrDefault();
        }

        public Product Save(Product item)
        {
            context.Products.Add(item);
            context.SaveChanges();

            return item;
        }

        public void Delete(Product item)
        {
            context.Products.Remove(item);
            context.SaveChanges();
        }

        
        public void Update(Product item)
        {
            context.Update(item);
            context.SaveChanges();
        }


    }
}