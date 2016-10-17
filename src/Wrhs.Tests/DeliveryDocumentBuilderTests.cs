using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Operations.Delivery;
using Wrhs.Products;

namespace Wrhs.Tests
{
    [TestFixture]
    public class DeliveryDocumentBuilderTests
    {
        [Test]
        public void BuildReturnsDeliveryDocument()
        {
            var builder = MakeBuilder();

            var document = builder.Build();

            Assert.IsInstanceOf<DeliveryDocument>(document);
        }

        public DeliveryDocumentBuilder MakeBuilder()
        {
            var repo = MakeProductRepository();
            var addLineValidMock = new Mock<IValidator<DocumentBuilderAddLineCommand>>();
            var updateLineValidMock = new Mock<IValidator<DocumentBuilderUpdateLineCommand>>();

            var builder = new DeliveryDocumentBuilder(repo, addLineValidMock.Object, updateLineValidMock.Object);
            return builder;
        }

        public IRepository<Product> MakeProductRepository()
        {
            return MakeProductRepository(20);
        }

        protected IRepository<Product> MakeProductRepository(int itemsCount)
        {
            var repo = RepositoryFactory<Product>.Make();
            var items = MakeItems(itemsCount);
            foreach(var item in items)
                repo.Save(item);
           
           return repo;
        }

        protected List<Product> MakeItems(int count)
        {
            var items = new List<Product>();
            for(var i=0; i<count; i++)
            {
                items.Add(new Product
                {
                    Id = i+1,
                    Code = $"PROD{i+1}",
                    Name = $"Product {i+1}",
                    EAN = $"000{i+1}"
                });
            }

            return items;
        }
    }
}