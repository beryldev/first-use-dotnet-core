using System;
using System.Linq;
using Wrhs.Products;

namespace Wrhs.Data
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var repo = new ProductRepository();
            var prod = new Product
            {
                Code = "PROD2",
                Name = "Product 2",
                EAN = "1111112",
                SKU = "1112",
                Description = "some desc"
            };

            repo.Save(prod);

            var items = repo.Get().ToArray();
            foreach(var item in items)
                Console.WriteLine(item.Code);
        }
    }
}
