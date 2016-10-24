using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wrhs.Core.Search;
using Wrhs.Products;

namespace Wrhs.Tests.Products
{
    [TestFixture]
    public class ProductSearchTests: ProductCommandTestsBase
    {
        [Test]
        [TestCase(5, 2)]
        [TestCase(10, 5)]
        [TestCase(20, 7)]
        [TestCase(321, 30)]
        public void OnSearchWithEmptyCriteriaReturnAllProducts(int itemsCount, int pageSize)
        {
            var items = MakeItems(itemsCount);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = search.MakeCriteria();
            criteria.PerPage = pageSize;

            var result = search.Exec(criteria);

            Assert.AreEqual(itemsCount, result.Total);
            Assert.LessOrEqual(pageSize, result.Items.Count());
        }

        [Test]
        [TestCase("Product 1")]
        [TestCase("Product 6")]
        [TestCase("Product 20")]
        public void OnSerachReturnProductsByName(string productName)
        {
            var items = MakeItems(20);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereName(Condition.Equal, productName);

            var result = search.Exec(criteria);

            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(1, result.Total);
            Assert.AreEqual(productName, result.Items.First().Name);
        }

        [Test]
        [TestCase("Product", 20)]
        [TestCase("Product 6", 1)]
        [TestCase("Product 1", 11)]
        public void OnSerachReturnProductsByNameContainsString(string productName, int count)
        {
            var items = MakeItems(20);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereName(Condition.Contains, productName);

            var result = search.Exec(criteria);

            Assert.AreEqual(count, result.Total);
        }

        [Test]
        [TestCase("PROD1")]
        [TestCase("PROD6")]
        [TestCase("PROD20")]
        public void OnSerachReturnProductsByCode(string productCode)
        {
            var items = MakeItems(20);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereCode(Condition.Equal, productCode);

            var result = search.Exec(criteria);

            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(1, result.Total);
            Assert.AreEqual(productCode, result.Items.First().Code);
        }

        [Test]
        [TestCase("PROD", 20)]
        [TestCase("PROD6", 1)]
        [TestCase("PROD1", 11)]
        public void OnSerachReturnProductsByCodeContainsString(string productCode, int count)
        {
            var items = MakeItems(20);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereCode(Condition.Contains, productCode);

            var result = search.Exec(criteria);

            Assert.AreEqual(count, result.Total);
        }

        [Test]
        [TestCase("0001")]
        [TestCase("00012")]
        [TestCase("00020")]
        public void OnSerachReturnProductsByEAN(string ean)
        {
            var items = MakeItems(20);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereEAN(Condition.Equal, ean);

            var result = search.Exec(criteria);

            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(1, result.Total);
            Assert.AreEqual(ean, result.Items.First().EAN);
        }

        [Test]
        [TestCase("000", 20)]
        [TestCase("0006", 1)]
        [TestCase("0001", 11)]
        public void OnSerachReturnProductsByEANContainsString(string ean, int count)
        {
            var items = MakeItems(20);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereEAN(Condition.Contains, ean);

            var result = search.Exec(criteria);

            Assert.AreEqual(count, result.Total);
        }

        protected List<Product> MakeItems(int count)
        {
            var items = new List<Product>();
            for(var i=0; i<count; i++)
            {
                items.Add(new Product
                {
                    Code = $"PROD{i+1}",
                    Name = $"Product {i+1}",
                    EAN = $"000{i+1}"
                });
            }

            return items;
        }
    }
}