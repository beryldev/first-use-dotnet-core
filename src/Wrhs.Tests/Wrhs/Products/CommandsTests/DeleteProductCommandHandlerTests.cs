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
            var repo = MakeProductRepository(MakeProductList());
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
            var repo = MakeProductRepository(MakeProductList());
            var handler = new DeleteProductCommandHandler(repo);
            var command = new DeleteProductCommand
            {
                ProductId = 3
            };

            handler.Handle(command);

            Assert.AreEqual(1, repo.Get().Count());
            Assert.AreEqual("PROD1", repo.Get().First().Code);
        }
    }
}