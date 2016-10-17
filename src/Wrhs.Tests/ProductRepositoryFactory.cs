using System.Collections.Generic;
using System.Linq;
using Moq;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Tests
{
    public static class ProductRepositoryFactory
    {
        public static IRepository<Product> Make()
        {
            var items = new List<Product>();
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


    public static class RepositoryFactory<T> where T : IEntity
    {
        public static IRepository<T> Make()
        {
             var items = new List<T>();
            var mock = new Mock<IRepository<T>>();
            mock.Setup(m=>m.Save(It.IsAny<T>()))
                .Callback((T item)=>{ items.Add(item); });

            mock.Setup(m=>m.Get())
                .Returns(items);

            mock.Setup(m=>m.GetById(It.IsAny<int>()))
                .Returns((int id)=>
                { 
                    return items.ToArray().Where(item=>item.Id==id).FirstOrDefault(); 
                });

            mock.Setup(m=>m.Update(It.IsAny<T>()))
                .Callback((T obj)=>
                {
                    var p = items.Where(item=>item.Id==obj.Id).FirstOrDefault();
                    if(p!=null)
                    {
                        items.Remove(p);
                        items.Add(obj);
                    }
                });

            mock.Setup(m=>m.Delete(It.IsAny<T>()))
                .Callback((T obj) =>
                {
                    items.Remove(obj);
                });

            return mock.Object;
        }
    }
}