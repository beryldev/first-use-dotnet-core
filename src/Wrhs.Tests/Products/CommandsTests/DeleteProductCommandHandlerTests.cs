using System.Linq;
using NUnit.Framework;
using Wrhs.Products;
using Wrhs.Products.Commands;

namespace Wrhs.Tests.Products
{
    [TestFixture]
    public class DeleteProductCommandHandlerTests : ProductCommandTestsBase
    {
        [Test]
        public void OnHandleRemoveProductFromRepository()
        {
            var items = MakeProductList();
            var repo = MakeProductRepository(items);
            var handler = new DeleteProductCommandHandler(repo);
            var command = new DeleteProductCommand
            {
                ProductId = 1
            };

            handler.Handle(command);

            CollectionAssert.IsEmpty(repo.Get());
        }

        [Test]
        public void OnHandleRemoveUnexistedProductRepositoryInUnchaged()
        {
            var items = MakeProductList();
            var repo = MakeProductRepository(items);
            var handler = new DeleteProductCommandHandler(repo);
            var command = new DeleteProductCommand
            {
                ProductId = 3
            };

            handler.Handle(command);

            Assert.AreEqual(1, items.Count());
            Assert.AreEqual("PROD1", items.First().Code);
        }
    }
}