using System.Collections.Generic;
using System.Linq;
using Moq;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Tests.Products
{
    public abstract class ProductCommandTestsBase
    {
        protected List<Product> MakeProductList()
        {
            var items = new List<Product>()
            {
                new Product
                {
                    Id = 1,
                    Code = "PROD1",
                    Name = "Product 1",
                    Description = "Some desc"
                }
            };

            return items;
        }
        
        protected IRepository<Product> MakeProductRepository(List<Product> items)
        {
             var mock = new Mock<IRepository<Product>>();
            mock.Setup(m=>m.Save(It.IsAny<Product>()))
                .Callback((Product prod)=>{ items.Add(prod); });

            mock.Setup(m=>m.Get())
                .Returns(items);

            mock.Setup(m=>m.GetById(It.IsAny<int>()))
                .Returns((int id)=>
                { 
                    return items.ToArray().Where(item=>item.Id==id).FirstOrDefault(); 
                });

            mock.Setup(m=>m.Update(It.IsAny<Product>()))
                .Callback((Product product)=>
                {
                    var p = items.Where(item=>item.Id==product.Id).FirstOrDefault();
                    if(p!=null)
                    {
                        items.Remove(p);
                        items.Add(product);
                    }
                });

            mock.Setup(m=>m.Delete(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    items.Remove(product);
                });

            return mock.Object;
        }
    }
}