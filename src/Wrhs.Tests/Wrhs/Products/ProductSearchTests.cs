using System.Collections.Generic;
using System.Linq;
using Wrhs.Core.Search;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class ProductSearchTests: ProductCommandTestsBase
    {
        [Theory]
        [InlineData(5, 2)]
        [InlineData(10, 5)]
        [InlineData(20, 7)]
        [InlineData(321, 30)]
        public void OnSearchWithEmptyCriteriaReturnAllProducts(int itemsCount, int pageSize)
        {
            var items = MakeItems(itemsCount);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = search.MakeCriteria();
            criteria.PerPage = pageSize;

            var result = search.Exec(criteria);

            Assert.Equal(itemsCount, result.Total);
            Assert.InRange<int>(result.Items.Count(), 0, pageSize);
        }

        [Theory]
        [InlineData("Product 1")]
        [InlineData("Product 6")]
        [InlineData("Product 20")]
        public void OnSerachReturnProductsByName(string productName)
        {
            var items = MakeItems(20);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereName(Condition.Equal, productName);

            var result = search.Exec(criteria);

            Assert.Equal(1, result.Items.Count());
            Assert.Equal(1, result.Total);
            Assert.Equal(productName, result.Items.First().Name);
        }

        [Theory]
        [InlineData("Product", 20)]
        [InlineData("Product 6", 1)]
        [InlineData("Product 1", 11)]
        [InlineData("product 1", 11)]
        public void OnSerachReturnProductsByNameContainsString(string productName, int count)
        {
            var items = MakeItems(20);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereName(Condition.Contains, productName);

            var result = search.Exec(criteria);

            Assert.Equal(count, result.Total);
        }

        [Theory]
        [InlineData("PROD1")]
        [InlineData("PROD6")]
        [InlineData("PROD20")]
        public void OnSerachReturnProductsByCode(string productCode)
        {
            var items = MakeItems(20);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereCode(Condition.Equal, productCode);

            var result = search.Exec(criteria);

            Assert.Equal(1, result.Items.Count());
            Assert.Equal(1, result.Total);
            Assert.Equal(productCode, result.Items.First().Code);
        }

        [Theory]
        [InlineData("PROD", 20)]
        [InlineData("PROD6", 1)]
        [InlineData("PROD1", 11)]
        [InlineData("prod1", 11)]
        public void OnSerachReturnProductsByCodeContainsString(string productCode, int count)
        {
            var items = MakeItems(20);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereCode(Condition.Contains, productCode);

            var result = search.Exec(criteria);

            Assert.Equal(count, result.Total);
        }

        [Theory]
        [InlineData("0001")]
        [InlineData("00012")]
        [InlineData("00020")]
        public void OnSerachReturnProductsByEAN(string ean)
        {
            var items = MakeItems(20);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereEAN(Condition.Equal, ean);

            var result = search.Exec(criteria);

            Assert.Equal(1, result.Items.Count());
            Assert.Equal(1, result.Total);
            Assert.Equal(ean, result.Items.First().EAN);
        }

        [Theory]
        [InlineData("000", 20)]
        [InlineData("0006", 1)]
        [InlineData("0001", 11)]
        public void OnSerachReturnProductsByEANContainsString(string ean, int count)
        {
            var items = MakeItems(20);
            var repo = MakeProductRepository(items);
            var search = new ResourceSearch<Product>(repo, new Paginator<Product>(),
                new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereEAN(Condition.Contains, ean);

            var result = search.Exec(criteria);

            Assert.Equal(count, result.Total);
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